using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateChapters : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentDropdown = GetComponent<TMP_Dropdown>();
        currentDropdown.options = Dropdown.options;
    }
}
