using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LoadText : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Text title;
    public TMP_Text text;
    public Button button;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { LoadTextValue(dropdown.value); });
    }

    public void LoadTextValue(int dropdownIndex)
    {
        string path = Path.Combine(UnityEngine.Application.persistentDataPath, "SaveData", title.text, "Chapters", dropdown.options[dropdownIndex].text);
        Debug.Log("File path: " + path);

        string filePath = Path.Combine(path, "data.txt");

        if (File.Exists(filePath))
        {
            string fileContents = File.ReadAllText(filePath);
            text.text = fileContents;
        }
        else
        {
            Debug.LogError("File not found at: " + filePath);
        }
    }
    public void SetDropDownValue()
    {
        Debug.Log("SetDropDownValue");
        dropdown.value = 0;
    }

    void Update()
    {

    }
}