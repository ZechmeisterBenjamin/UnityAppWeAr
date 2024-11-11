using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;
using System.Globalization;

public class LoadImportEntries : MonoBehaviour
{
    public GameObject Prefab;
    public ScrollView scrollView; // Declare a ScrollView variable

    void Start()
    {
        // Start fetching projects
        StartCoroutine(ProjectFetcher.GetProjects(ProjectsReceived));
    }

    void ProjectsReceived(FetchedProject[] projects)
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

    public string FormatDate(string dateTimeString)
    {
        // Parse the input string to a DateTime object
        DateTime dateTime = DateTime.Parse(dateTimeString, null, DateTimeStyles.RoundtripKind);

        // Format the date to day.month.year
        return dateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    }
}
