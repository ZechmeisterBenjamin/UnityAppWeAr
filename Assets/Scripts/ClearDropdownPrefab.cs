using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearDropdownPrefab : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    void OnApplicationQuit()
    {
        dropdown.options = null;
    }
}
