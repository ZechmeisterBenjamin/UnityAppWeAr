using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

public class LoadImportEntries : MonoBehaviour
{
    public GameObject prefab; // The prefab for each project entry
    public Transform scrollViewContent; // The content area of the ScrollView
    public TMP_InputField searchBar; // TMP_InputField to function as a search bar

    // List of all entries
    private List<GameObject> allEntries = new List<GameObject>();
    private List<FetchedProject> allProjects = new List<FetchedProject>();

    void Start()
    {
        // Start fetching projects
        StartCoroutine(ProjectFetcher.GetProjects(ProjectsReceived));

        if (searchBar != null)
        {
            searchBar.onValueChanged.AddListener(OnSearchValueChanged);
        }
        else
        {
            Debug.LogError("Search bar is not assigned.");
        }
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

        allProjects.AddRange(projects);

        // Get the height of a single prefab entry
        RectTransform prefabRect = prefab.GetComponent<RectTransform>();
        float prefabHeight = prefabRect.rect.height;
        float spacing = 5f; // Optional spacing between entries

        // Calculate the total height needed for the content
        float totalHeight = (prefabHeight + spacing) * projects.Length - spacing;
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
            if (projects.Length == 1)
            {
                yPos = 0; // Center the single entry
            }
            entryRect.anchoredPosition = new Vector2(0, yPos);

            // Add the entry to the list of all entries
            allEntries.Add(projectEntry);
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

    // Method to be called when the search bar value changes
    private void OnSearchValueChanged(string searchTerm)
    {
        // Clear current content
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        // Get the height of a single prefab entry
        RectTransform prefabRect = prefab.GetComponent<RectTransform>();
        float prefabHeight = prefabRect.rect.height;
        float spacing = 5f; // Optional spacing between entries

        // Filter and re-add entries that match the search term
        var filteredProjects = allProjects.FindAll(project => project.projectname.ToLower().Contains(searchTerm.ToLower()));
        float totalHeight = (prefabHeight + spacing) * filteredProjects.Count - spacing;
        RectTransform contentRect = scrollViewContent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);

        for (int i = 0; i < filteredProjects.Count; i++)
        {
            var project = filteredProjects[i];

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
                if(File.Exists(savePath))
                    File.Delete(savePath);
                StartCoroutine(Download(project, savePath));
            });

            // Set the position of the prefab in the content area
            RectTransform entryRect = projectEntry.GetComponent<RectTransform>();
            float yPos = -i * (prefabHeight + spacing);
            if (filteredProjects.Count == 1)
            {
                yPos = -prefabHeight / 2 - 3; // Move the single entry down a bit
            }
            entryRect.anchoredPosition = new Vector2(0, yPos);
        }
    }

}
