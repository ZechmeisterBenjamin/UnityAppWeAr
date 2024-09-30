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
        Debug.LogWarning("Path: " + path);

        // Create folder if not exist
        if (!System.IO.Directory.Exists(path + "/SaveData"))
        {
            System.IO.Directory.CreateDirectory(path + "/SaveData");
            Debug.LogWarning("Test");
        }



    }
}