using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath;
        Debug.Log("Path: " + path);
        Debug.Log(Application.persistentDataPath);
        // Create folder if not exist
        if (!System.IO.Directory.Exists(path + "\\SaveData"))
        {
            System.IO.Directory.CreateDirectory(path + "\\SaveData");
        }

        ReadSaveData readSaveData = GetComponent<ReadSaveData>();

        if (readSaveData != null)
        {
            readSaveData.Read();  // Call a method from ReadSaveData if needed
        }
        else
        {
            Debug.LogWarning("ReadSaveData script is not attached to this GameObject.");
        }
    }
}
