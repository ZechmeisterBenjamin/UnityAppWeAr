using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ReadSaveData : MonoBehaviour
{
    // Method to read files in the folder
    public void Read()
    {
        // Read all files in the folder Application.persistentDataPath + "/SaveData"
        string path = Application.persistentDataPath + "/SaveData";
        
        List<string> dir = Directory.GetDirectories(path).ToList();
        foreach (var f in dir)
        { Debug.Log(f); }
    }
}
