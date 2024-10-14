using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoadProjects : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;
    public TMP_InputField inputField;
    private List<GameObject> buttonPool = new List<GameObject>(); // Pool of buttons
    private List<Project> allProjects = new List<Project>();

    private Coroutine filterCoroutine;
    private float debounceTime = 0.1f; // Time in seconds to wait before filtering

    void Start()
    {
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

        // Subscribe to input field text change event
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    // Callback method to handle loading the projects into buttons
    private void OnProjectsLoaded(List<Project> projects)
    {
        allProjects = projects;
        FilterAndDisplayProjects();
    }

    // Filters projects based on input text and displays them
    private void FilterAndDisplayProjects()
    {
        string filterText = inputField.text;

        // Hide all buttons from the pool
        foreach (var button in buttonPool)
        {
            button.SetActive(false);
        }

        int index = 0;
        foreach (var project in allProjects)
        {
            if (project.Name.ToLower().Contains(filterText.ToLower()))
            {
                GameObject newButton;

                // Reuse a button from the pool if available
                if (index < buttonPool.Count)
                {
                    newButton = buttonPool[index];
                }
                else
                {
                    newButton = Instantiate(buttonPrefab, content);
                    buttonPool.Add(newButton);
                }

                newButton.SetActive(true);
                TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    // Set font size to match black buttons
                    buttonText.fontSize = 20;
                    buttonText.enableAutoSizing = false; // Ensure that text does not auto-scale
                    buttonText.alignment = TextAlignmentOptions.Left; // Adjust alignment to match the look
                    buttonText.text = project.Name;
                }
                else
                {
                    Debug.LogError("No TextMeshProUGUI component found on button prefab!");
                }

                // Add the BlockDrag component to prevent dragging
                if (newButton.GetComponent<BlockDrag>() == null)
                {
                    newButton.AddComponent<BlockDrag>();
                }

                index++;
            }
        }
    }

    // Called when the input field value changes
    private void OnInputValueChanged(string newValue)
    {
        // Debounce input to prevent filtering too often
        if (filterCoroutine != null)
        {
            StopCoroutine(filterCoroutine);
        }

        filterCoroutine = StartCoroutine(DebounceFilter());
    }

    // Coroutine to delay filtering
    private IEnumerator DebounceFilter()
    {
        yield return new WaitForSeconds(debounceTime);
        FilterAndDisplayProjects();
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
}
