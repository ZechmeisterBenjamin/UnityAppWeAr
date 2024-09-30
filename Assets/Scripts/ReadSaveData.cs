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
        string path = Application.persistentDataPath + "\\SaveData";
        List<string> dir = Directory.GetDirectories(path).ToList();
        foreach (var f in dir)
        { Debug.Log(PathToProjectName(f)); }
    }
    private string PathToProjectName(string path)
    {
        string[] parts = path.Split('\\');
        return parts[parts.Length - 1];
    }
}
