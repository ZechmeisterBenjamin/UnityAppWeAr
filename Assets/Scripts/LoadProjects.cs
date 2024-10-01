using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LoadProjects : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;

    void Start()
    {
        Debug.Log("LoadProjects Start");

        // Get the ReadSaveData component
        ReadSaveData readSaveData = GetComponent<ReadSaveData>();

        if (readSaveData != null)
        {
            // Start the reading process and pass a callback function to handle project loading after data is ready
            StartCoroutine(readSaveData.Read(OnProjectsLoaded));
        }
        else
        {
            Debug.LogError("ReadSaveData component not found on this GameObject.");
        }
    }

    // Callback method to handle loading the projects into buttons
    private void OnProjectsLoaded(List<string> projects)
    {
        foreach (var project in projects)
        {
            GameObject newButton = Instantiate(buttonPrefab, content);
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = project;
            }
            else
            {
                Debug.LogError("No TextMeshProUGUI component found on button prefab!");
            }
        }
    }
}
