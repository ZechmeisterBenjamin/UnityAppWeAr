using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.Mime.MediaTypeNames;

[Serializable]
public class FetchedProject
{
    public string projectname; // Match JSON keys
    public string description;
    public string last_changed; // Change to string to avoid DateTime issues
    public string path_for_zipfile;
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

                FetchedProject[] projects = JsonHelper.FromJson<FetchedProject>(jsonResponse);
                callback(projects);
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


[Serializable]
public class FetchedProjectArray
{
    public FetchedProject[] projects;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
