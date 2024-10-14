using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ReadSaveData : MonoBehaviour
{
    public IEnumerator Read(System.Action<List<Project>> callback)
    {
        string path = Application.persistentDataPath + "\\SaveData";
        List<string> dir = Directory.GetDirectories(path).ToList();
        List<Project> projects = new List<Project>();

        foreach (var f in dir)
        {
            Project project = new Project();
            project.Name = PathToProjectName(f);
            project.Chapters = GetChapters(f); // Übergib den Pfad des Projekts an GetChapters
            projects.Add(project);
            yield return null; // Yield, um anderen Prozessen Zeit zu geben
        }

        // Invoke callback once data reading is finished
        callback?.Invoke(projects);
    }

    private string PathToProjectName(string path)
    {
        string[] parts = path.Split('\\');
        return parts[parts.Length - 1];
    }

    private List<Chapter> GetChapters(string projectPath)
    {
        List<Chapter> chapters = new List<Chapter>();

        // Stelle sicher, dass der Kapitel-Pfad korrekt ist
        string chaptersPath = Path.Combine(projectPath, "Chapters");

        if (Directory.Exists(chaptersPath))
        {
            foreach (var f in Directory.GetDirectories(chaptersPath))
            {
                Chapter chapter = new Chapter();
                chapter.Name = PathToProjectName(f);
                chapters.Add(chapter);
            }
        }
        else
        {
            Debug.LogWarning($"Kapitelverzeichnis nicht gefunden: {chaptersPath}");
        }

        return chapters;
    }
}