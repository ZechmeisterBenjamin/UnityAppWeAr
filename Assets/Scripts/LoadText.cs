using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadText : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Text title;
    public TMP_Text text;

    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { LoadTextValue(); });
        LoadTextValue();
    }

    public void LoadTextValue()
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveData", title.text, "Chapters", dropdown.options[dropdown.value].text);
        Debug.Log("File path: " + path);

        // Combine the path with the file name
        string filePath = Path.Combine(path, "data.txt");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the contents of the file
            string fileContents = File.ReadAllText(filePath);
            text.text = fileContents;
        }
        else
        {
            Debug.LogError("File not found at: " + filePath);
        }
    }

    void Update()
    {
        
    }
}
