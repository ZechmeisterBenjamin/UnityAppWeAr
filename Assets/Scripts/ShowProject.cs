using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
public class ShowProject : MonoBehaviour
{
    public object obj;
    public Button button;
    public TMP_Dropdown dropdown;
    public TMP_Text text;
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
            text.text = projectName;
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
        else
        {
            Debug.LogError("Kein TextMeshProUGUI-Komponente im Button gefunden!");
        }
    }
    private void OnProjectsLoaded(List<Project> projects)
    {
        Debug.Log($"Dropdown reference: {dropdown.name}");
        if (projects == null || projects.Count == 0)
        {
            Debug.LogWarning("Keine Projekte geladen.");
            return;
        }

        List<Chapter> chapters = new List<Chapter>();
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var project in projects)
        {
            Debug.Log($"Projekt gefunden: {project.Name}"); // Debugging für Projekte
            Debug.Log(projectName);
            if (project.Name == projectName)
            {
                foreach (var chapter in project.Chapters)
                {
                    chapters.Add(chapter);
                    Debug.Log($"Kapitel hinzugefügt: {chapter.Name}"); // Debugging für Kapitel
                }
                break;
            }
        }

        if (chapters.Count == 0)
        {
            Debug.LogWarning($"Keine Kapitel für das Projekt '{projectName}' gefunden.");
            return;
        }

        foreach (var chapter in chapters)
        {
            options.Add(new TMP_Dropdown.OptionData(chapter.Name));
        }
        dropdown.ClearOptions();
        dropdown.options = options;
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }
}
