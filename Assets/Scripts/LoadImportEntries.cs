using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using System.Collections;

public class LoadImportEntries : MonoBehaviour
{
    public GameObject prefab; // The prefab for each project entry
    public Transform scrollViewContent; // The content area of the ScrollView

    void Start()
    {
        // Start fetching projects
        StartCoroutine(ProjectFetcher.GetProjects(ProjectsReceived));
    }

    void ProjectsReceived(FetchedProject[] projects)
    {
        if (projects == null)
        {
            Debug.LogError("Projects array is null.");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned.");
            return;
        }

        if (scrollViewContent == null)
        {
            Debug.LogError("ScrollViewContent is not assigned.");
            return;
        }

        // Get the height of a single prefab entry
        RectTransform prefabRect = prefab.GetComponent<RectTransform>();
        float prefabHeight = prefabRect.rect.height;
        float spacing = 5f; // Optional spacing between entries

        // Calculate the total height needed for the content
        float totalHeight = (prefabHeight + spacing) * projects.Length;
        RectTransform contentRect = scrollViewContent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);

        for (int i = 0; i < projects.Length; i++)
        {
            var project = projects[i];

            // Instantiate the prefab and set it as a child of the scrollViewContent
            GameObject projectEntry = Instantiate(prefab, scrollViewContent);

            if (projectEntry == null)
            {
                Debug.LogError("Failed to instantiate project entry prefab.");
                continue;
            }

            // Find and set the UI elements of the prefab
            TMP_Text projectNameText = projectEntry.transform.Find("Canvas/Project Name")?.GetComponent<TMP_Text>();
            TMP_Text lastChangedText = projectEntry.transform.Find("Canvas/Last Changed")?.GetComponent<TMP_Text>();
            Button downloadButton = projectEntry.transform.Find("Canvas/Download")?.GetComponent<Button>();

            if (projectNameText == null || lastChangedText == null || downloadButton == null)
            {
                Debug.LogError("Failed to find UI components in the prefab.");
                continue;
            }

            projectNameText.text = project.projectname;
            lastChangedText.text = FormatDate(project.last_changed);

            string savePath = Path.Combine(Application.persistentDataPath + @"\SaveData", project.projectname + ".zip");
            downloadButton.onClick.AddListener(() =>
            {
                Debug.Log("Downloading " + projectNameText.text);
                StartCoroutine(Download(project, savePath));
            });
            // Set the position of the prefab in the content area
            RectTransform entryRect = projectEntry.GetComponent<RectTransform>();
            float yPos = -i * (prefabHeight + spacing);
            entryRect.anchoredPosition = new Vector2(0, yPos);
        }
    }

    IEnumerator Download(FetchedProject project, string savePath)
    {
        Debug.Log("Wallah download");
        yield return StartCoroutine(ProjectFetcher.DownloadZipFile(project.path_for_zipfile, savePath));
        System.IO.Compression.ZipFile.ExtractToDirectory(savePath, savePath.Substring(0, savePath.Length - 4));
        File.Delete(savePath);
    }
    public string FormatDate(string dateTimeString)
    {
        // Parse the input string to a DateTime object
        DateTime dateTime = DateTime.Parse(dateTimeString, null, DateTimeStyles.RoundtripKind);

        // Format the date to day.month.year
        return dateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    }
}
