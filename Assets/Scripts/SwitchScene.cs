using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // Scene asset to switch to
    public Object scene;

    // Button to attach the listener to
    public Button switchButton;

    private void Start()
    {
        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchScene);
        }
        else
        {
            Debug.LogError("Switch button is not set.");
        }
    }

    // Method to be called when the button is clicked
    public void SwitchScene()
    {
        if (scene != null)
        {
            string sceneName = scene.name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene is not set.");
        }
    }
}
