using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class FetchedProject
{
    public string projectname;
    public string description;
    public string last_changed;
    public string path_for_zipfile;
}

[Serializable]
public class FetchedProjectResponse
{
    public int count;
    public string next;
    public string previous;
    public FetchedProject[] results;
}

public class ProjectFetcher : MonoBehaviour
{
    private static string configFilePath = Path.Combine(Application.persistentDataPath, "SaveData", "config.cfg");

    public static IEnumerator GetProjects(Action<FetchedProject[]> callback)
    {
        (string uri, string passcode) = ReadConfigFile();
        if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(passcode))
        {
            Debug.LogError("URI or passcode not found in config file.");
            callback(null);
            yield break;
        }

        using (var request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-Passcode", passcode);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);

                // Deserialize the JSON response
                FetchedProjectResponse response = JsonUtility.FromJson<FetchedProjectResponse>(jsonResponse);
                if (response != null && response.results != null)
                {
                    foreach (var project in response.results)
                    {
                        Debug.Log($"Project Name: {project.projectname}");
                        Debug.Log($"Description: {project.description}");
                        Debug.Log($"Last Changed: {project.last_changed}");
                        Debug.Log($"Path for Zipfile: {project.path_for_zipfile}");
                        Debug.Log("");
                    }

                    callback(response.results);
                }
                else
                {
                    Debug.LogError("Failed to parse JSON response.");
                    callback(null);
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                callback(null);
            }
        }
    }

    public static IEnumerator DownloadZipFile(string url, string savePath)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                byte[] fileData = request.downloadHandler.data;
                File.WriteAllBytes(savePath, fileData);
                Debug.Log($"File downloaded and saved to {savePath}");
            }
            else
            {
                Debug.LogError("Failed to download file: " + request.error);
            }
        }
    }

    private static (string, string) ReadConfigFile()
    {
        string uri = null;
        string passcode = null;

        Debug.Log($"Reading config file from path: {configFilePath}");

        if (File.Exists(configFilePath))
        {
            string[] lines = File.ReadAllLines(configFilePath);
            foreach (string line in lines)
            {
                Debug.Log($"Config line: {line}");
                if (line.Contains("api-link"))
                {
                    uri = line.Substring("api-link = ".Length).Trim().Trim('"');
                    Debug.Log($"Found URI: {uri}");
                }
                else if (line.Contains("api-passcode"))
                {
                    passcode = line.Substring("api-passcode = ".Length).Trim().Trim('"');
                    Debug.Log($"Found passcode: {passcode}");
                }
            }
        }
        else
        {
            Debug.LogError($"Config file not found at {configFilePath}");
        }

        Debug.Log($"Final URI: {uri}");
        Debug.Log($"Final Passcode: {passcode}");

        return (uri, passcode);
    }
}
