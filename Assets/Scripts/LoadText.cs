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
            List<string> categoryTexts = SplitTextIntoCategories(fileContents);
            text.text = categoryTexts[0];
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
    private List<string> SplitTextIntoCategories(string str)
    {
        List<string> strings = new List<string>();
        int i = 0;

        while (true)
        {
            try
            {
                string startTag = $"[$%{{{i}}}%$]";
                string endTag = $"]$%{{{i}}}%$[";
                int startIndex = str.IndexOf(startTag);
                int endIndex = str.IndexOf(endTag);

                if (startIndex == -1 || endIndex == -1)
                {
                    Debug.LogError(startTag);
                    break;
                }

                strings.Add(str.Substring(startIndex + startTag.Length, endIndex - endTag.Length));

                i++;
            }
            catch
            {
                Debug.LogError(str);
                break;
            }
        }

        return strings;
    }

    void Update()
    {

    }
}