using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateProjectTitle : MonoBehaviour
{
    public TMP_Text TMP_Text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var text = GetComponent<TMP_Text>();
        text.text = TMP_Text.text;
    }
}
