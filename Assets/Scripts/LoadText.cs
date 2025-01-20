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
    public TMP_Text category;
    public Button button;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { LoadTextValue(dropdown.value); });

        category.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { CategoryTextChanged(); });
    }


    public void CategoryTextChanged()
    {
        LoadTextValue(dropdown.value); // Reload text when the category is changed
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
            text.text = categoryTexts[int.Parse(category.text)-1];
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
            string startTag = $"[$%{{{i}}}%$]";
            string endTag = $"]$%{{{i}}}%$[";

            int startIndex = str.IndexOf(startTag);
            int endIndex = str.IndexOf(endTag);

            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            {
                if (startIndex != -1) Debug.LogError($"Missing end tag {endTag} in string: " + str);
                if (endIndex != -1) Debug.LogError($"Missing start tag {startTag} in string: " + str);
                if (endIndex <= startIndex) Debug.LogError($"End tag {endTag} found before start tag {startTag} in string: " + str);
                break;
            }

            int substringStart = startIndex + startTag.Length;
            int substringLength = endIndex - substringStart;

            if (substringLength < 0)
            {
                Debug.LogError($"invalid tag found: start at index {startIndex} and end at {endIndex}. Full string: " + str);
                break;
            }

            string extractedString = str.Substring(substringStart, substringLength);
            strings.Add(extractedString);
            Debug.Log($"Extracted string: {extractedString}");

            str = str.Substring(endIndex + endTag.Length);

            i++;
        }

        return strings;
    }

    void Update()
    {

    }
}