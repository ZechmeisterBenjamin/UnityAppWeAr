using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowProject : MonoBehaviour
{
    public object obj;
    public Button button;
    public TMP_Dropdown dropdown;
    private string projectName = "";

    void Start()
    {
      
        if (button == null || dropdown == null)
        {
            Debug.LogError("Button oder Dropdown ist nicht zugewiesen!");
            return;
        }

        button.onClick.AddListener(TaskOnClick);

        ReadSaveData readSaveData = GetComponent<ReadSaveData>();
        if (readSaveData != null)
        {
            StartCoroutine(readSaveData.Read(OnProjectsLoaded));
        }
        else
        {
            Debug.LogError("ReadSaveData component not found on this GameObject.");
        }
    }

    void TaskOnClick()
    {
        Debug.Log("ShowProject Start");
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            projectName = buttonText.text;
            Debug.Log($"Projektname gesetzt auf: {projectName}");
        }
        else
        {
            Debug.LogError("Kein TextMeshProUGUI-Komponente im Button gefunden!");
        }
    }

    private void OnProjectsLoaded(List<Project> projects)
    {
        if (projects == null || projects.Count == 0)
        {
            Debug.LogWarning("Keine Projekte geladen.");
            return;
        }

        List<Chapter> chapters = new List<Chapter>();
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var project in projects)
        {
            Debug.Log($"Projekt gefunden: {project.Name}"); // Debugging f�r Projekte

            if (project.Name == projectName)
            {
                foreach (var chapter in project.Chapters)
                {
                    chapters.Add(chapter);
                    Debug.Log($"Kapitel hinzugef�gt: {chapter.Name}"); // Debugging f�r Kapitel
                }
                break;
            }
        }

        if (chapters.Count == 0)
        {
            Debug.LogWarning($"Keine Kapitel f�r das Projekt '{projectName}' gefunden.");
            return;
        }

        foreach (var chapter in chapters)
        {
            options.Add(new TMP_Dropdown.OptionData(chapter.Name));
        }

        dropdown.ClearOptions(); // Leere die aktuellen Optionen
        dropdown.AddOptions(options); // F�ge neue Optionen hinzu
    }
}
