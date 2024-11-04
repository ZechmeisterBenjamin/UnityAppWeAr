using System.IO;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Start fetching projects
        StartCoroutine(ProjectFetcher.GetProjects(OnProjectsReceived));
    }

    void OnProjectsReceived(FetchedProject[] projects)
    {
        if (projects != null)
        {
            foreach (var project in projects)
            {
                Debug.Log($"Project Name: {project.projectname}");
                Debug.Log($"Description: {project.description}");
                Debug.Log($"Last Changed: {project.last_changed}");
                Debug.Log($"Path for Zipfile: {project.path_for_zipfile}");
                Debug.Log("");

                // Define the path in Application.persistentDataPath
                string savePath = Path.Combine(UnityEngine.Application.persistentDataPath + @"\SaveData", project.projectname + ".zip");
                StartCoroutine(ProjectFetcher.DownloadZipFile(project.path_for_zipfile, savePath));
            }
        }
        else
        {
            Debug.LogError("Failed to fetch projects.");
        }
    }
}
