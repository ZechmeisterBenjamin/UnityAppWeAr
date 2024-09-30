using Microsoft.Unity.VisualStudio.Editor;
using PdfSharp.Pdf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project : MonoBehaviour
{
    public List<Chapter> Chapters { get; set; }
}
public class Chapter
{
    public string Name { get; set; }
    public string Data { get; set; }
    public List<Image> Images { get; set; }
    public List<Object> Objects { get; set; }
    public List<Animation> Animations { get; set; }
    public List<PdfDocument> PDFs { get; set; }
}


