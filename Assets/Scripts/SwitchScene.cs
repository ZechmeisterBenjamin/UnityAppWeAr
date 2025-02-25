using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneSwitcher : MonoBehaviour
{
    public Object scene;

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

    public void SwitchScene()
    {
        if (scene != null)
        {
            string sceneName = scene.name;
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.LogError("Scene is not set.");
        }
    }


    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Finished loading: " + sceneName);
    }

}