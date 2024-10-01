using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ReadSaveData : MonoBehaviour
{
    public IEnumerator Read(System.Action<List<string>> callback)
    {
        string path = Application.persistentDataPath + "\\SaveData";
        List<string> dir = Directory.GetDirectories(path).ToList();
        List<string> projectNames = new List<string>();

        foreach (var f in dir)
        {
            projectNames.Add(PathToProjectName(f));
            Debug.Log(PathToProjectName(f)); // Log for debugging
            yield return null; // Yield to allow other processes to continue
        }

        // Invoke callback once data reading is finished
        callback?.Invoke(projectNames);
    }

    private string PathToProjectName(string path)
    {
        string[] parts = path.Split('\\');
        return parts[parts.Length - 1];
    }
}
