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

    private IEnumerator Start()
    {
        dropdown.onValueChanged.AddListener(delegate { LoadTextValue(dropdown.value); });
        yield return new WaitForSeconds(0.1f);
        dropdown.value = -1;
        Debug.Log("Set value to 1");
        yield return new WaitForSeconds(0.1f);
        dropdown.value = 0;
        Debug.Log("Set value to 0");
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

    void Update()
    {

    }
}
