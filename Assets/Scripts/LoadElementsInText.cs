using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWithDynamicImage : MonoBehaviour
{
    public TMP_Text content; // Reference to the TextMeshPro text object
    public TMP_Text directoryNameText; // Reference to the TextMeshPro text object containing the directory name

    private string lastProcessedText;

    void Start()
    {
        // Get the directory name from the TMP_Text object
        string directoryName = directoryNameText.text.Trim();
        string imagesDirectory = GetImagesDirectory(directoryName);

        if (string.IsNullOrEmpty(imagesDirectory))
        {
            return;
        }

        // Process the initial content text
        ProcessAndSetText(imagesDirectory);
    }

    void Update()
    {
        // Get the directory name from the TMP_Text object
        string directoryName = directoryNameText.text.Trim();
        string imagesDirectory = GetImagesDirectory(directoryName);

        if (string.IsNullOrEmpty(imagesDirectory))
        {
            return;
        }

        // Process the updated content text
        ProcessAndSetText(imagesDirectory);
    }

    void ProcessAndSetText(string imagesDirectory)
    {
        if (content == null)
        {
            Debug.LogError("Content TMP_Text is not assigned.");
            return;
        }

        string text = content.text;

        // Avoid processing if the text hasn't changed
        if (text == lastProcessedText)
        {
            return;
        }

        // Clear all children of the content object
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // Process the content text
        text = ProcessText(text, imagesDirectory);

        // Update the TextMeshPro content
        content.text = text;

        // Store the processed text to avoid reprocessing
        lastProcessedText = text;
    }

    string ProcessText(string inputText, string imagesDirectory)
    {
        // Match placeholders in the format [$%FILENAME.png%$]
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\[\$\%(.*?)\.png\%\$\]");
        System.Text.RegularExpressions.Regex regexObj = new System.Text.RegularExpressions.Regex(@"\[\$\%(.*?)\.obj\%\$\]");

        var matches = regex.Matches(inputText);
        var objMatches = regexObj.Matches(inputText);

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            string imageName = match.Groups[1].Value;
            string imagePath = Path.Combine(imagesDirectory, imageName + ".png");

            Debug.Log($"Looking for image at path: {imagePath}");

            if (File.Exists(imagePath))
            {
                // Load the image and create an Image component
                Sprite sprite = LoadSpriteFromFile(imagePath);
                if (sprite != null)
                {
                    // Determine where the placeholder is in the text
                    int placeholderIndex = inputText.IndexOf(match.Value);
                    Vector2 charPosition = GetTextPosition(content, placeholderIndex);

                    // Create a new GameObject with an Image component
                    GameObject imageObject = new GameObject(imageName);
                    Image imageComponent = imageObject.AddComponent<Image>();
                    imageComponent.sprite = sprite;

                    // Set the parent of the new image to the content object
                    imageObject.transform.SetParent(content.transform, false);

                    // Adjust the RectTransform of the image
                    RectTransform imageRectTransform = imageObject.GetComponent<RectTransform>();
                    float scaleFactor = 1.0f; // Adjust this value to scale the image
                    imageRectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height) * scaleFactor;
                    imageComponent.preserveAspect = true;

                    // Set the Y position to 0
                    imageRectTransform.anchoredPosition = new Vector2(0, 0);

                    // Add line breaks before and after the placeholder to prevent overlap
                    float imageHeight = sprite.rect.height * scaleFactor;
                    float lineHeight = content.fontSize * 1.2f; // Adjust based on your font settings
                    int lineBreaksBefore = Mathf.CeilToInt(imageHeight / lineHeight) + 1;
                    string lineBreaksBeforeString = new string('\n', lineBreaksBefore);

                    inputText = inputText.Replace(match.Value, $"{lineBreaksBeforeString}");

                    // Ensure the image appears in the correct order
                    imageObject.transform.SetSiblingIndex(content.transform.childCount - 1);
                }
                else
                {
                    Debug.LogError($"Failed to load image at path: {imagePath}");
                    inputText = inputText.Replace(match.Value, "[Image Unavailable]");
                }
            }
            else
            {
                Debug.LogError($"Image file not found at path: {imagePath}");
                inputText = inputText.Replace(match.Value, "[Image Not Found]");
            }
        }


        return inputText;
    }

    Vector2 GetTextPosition(TMP_Text textComponent, int charIndex)
    {
        TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[charIndex];
        Vector2 charPos = charInfo.bottomLeft;
        return charPos;
    }

    Sprite LoadSpriteFromFile(string imagePath)
    {
        Debug.Log($"Attempting to load image from path: {imagePath}");

        if (!File.Exists(imagePath))
        {
            Debug.LogError($"File does not exist: {imagePath}");
            return null;
        }

        byte[] fileData = File.ReadAllBytes(imagePath);
        Debug.Log($"File data loaded. Byte count: {fileData.Length}");

        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            Debug.Log($"Successfully loaded texture from file: {imagePath}");
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError($"Failed to load texture from file: {imagePath}");
            return null;
        }
    }

    string GetImagesDirectory(string directoryName)
    {
        string basePath = Path.Combine(Application.persistentDataPath, "SaveData");
        try
        {

       
        string[] directories = Directory.GetDirectories(basePath, directoryName, SearchOption.AllDirectories);

        foreach (string dir in directories)
        {
            string imagesPath = Path.Combine(dir, "Images");
            if (Directory.Exists(imagesPath))
            {
                return imagesPath;
            }
        }

        return null;
        }
        catch
        {
            return null;
        }
    }
}
