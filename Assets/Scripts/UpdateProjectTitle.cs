using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateProjectTitle : MonoBehaviour
{
    public TMP_Text TMP_Text;
    public TMP_Text LastEdit;
    void Start()
    {
        
    }

    void Update()
    {
        var text = GetComponent<TMP_Text>();
        text.text = TMP_Text.text == "New Text" ? string.Empty : TMP_Text.text;
        string folderPath = Application.persistentDataPath + "/SaveData/" + TMP_Text.text;
        Debug.Log("FolderPath " + folderPath);
        if (System.IO.Directory.Exists(folderPath))
        {
            var directoryInfo = new System.IO.DirectoryInfo(folderPath);
            LastEdit.text = "Stand: " + directoryInfo.LastWriteTime.Date.ToString("dd.MM.yyyy");
        }



    }
}
