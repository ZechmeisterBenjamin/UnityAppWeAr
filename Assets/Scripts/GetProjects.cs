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
    public static IEnumerator GetProjects(Action<FetchedProject[]> callback)
    {
        string uri = "https://api.htlwy.at/gaw/projects/";
        string passcode = "V-ck4E6gSxQ8G>LAawF=+r#PnYRNsv";
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
}
