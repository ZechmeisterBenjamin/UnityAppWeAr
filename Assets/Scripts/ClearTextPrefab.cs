using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearTextPrefab : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void OnApplicationQuit()
    {
        text.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
