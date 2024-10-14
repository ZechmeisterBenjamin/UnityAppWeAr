using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class LoadProjects : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;
    public TMP_InputField inputField;

    void Start()
    {
        while (content.childCount != 0)
        {
            Destroy(content.GetChild(0));
        }

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
            if (inputField.text == "" && project.Contains(inputField.text))
            {
                
            
            GameObject newButton = Instantiate(buttonPrefab, content);

            // Set button text
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                // Set font size to match black buttons
                buttonText.fontSize = 20;
                buttonText.enableAutoSizing = false; // Ensure that text does not auto-scale
                buttonText.alignment = TextAlignmentOptions.Left; // Adjust alignment to match the look

                buttonText.text = project;
            }
            else
            {
                Debug.LogError("No TextMeshProUGUI component found on button prefab!");
            }

            // Add the BlockDrag component to prevent dragging
            BlockDrag blockDrag = newButton.AddComponent<BlockDrag>();
            }
        }
    }
}

// This script blocks dragging of the buttons
public class BlockDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Prevent dragging
        eventData.pointerDrag = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Do nothing
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Do nothing
    }
}
