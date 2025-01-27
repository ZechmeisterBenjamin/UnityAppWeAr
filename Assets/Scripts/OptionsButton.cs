using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    public Button optionsButton;
    void Start()
    {
        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OpenConfigFile);
        }
        else
        {
            Debug.LogError("OptionsButton reference is not set.");
        }
    }

    void OpenConfigFile()
    {
        string configFilePath = Path.Combine(Application.persistentDataPath, "SaveData", "config.cfg");

        if (File.Exists(configFilePath))
        {
            Application.OpenURL("file://" + configFilePath);
        }
        else
        {
            File.WriteAllText(configFilePath, "api-link = \"\"\napi-passcode = \"\"");
            Application.OpenURL("file://" + configFilePath);
            Debug.LogError("Config file not found at path: " + configFilePath);
        }
    }
}
