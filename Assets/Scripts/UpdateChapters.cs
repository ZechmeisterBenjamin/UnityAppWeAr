using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateChapters : MonoBehaviour
{
    public TMP_Dropdown Dropdown; // The external dropdown

    private TMP_Dropdown currentDropdown;
    private bool optionsUpdated = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dropdown at the start
        currentDropdown = GetComponent<TMP_Dropdown>();
        if (Dropdown != null && currentDropdown != null)
        {
            // Copy the options from Dropdown to currentDropdown
            currentDropdown.options = new List<TMP_Dropdown.OptionData>(Dropdown.options);
            currentDropdown.value = Dropdown.value;

            // Add a listener to handle changes in currentDropdown
            currentDropdown.onValueChanged.AddListener(OnCurrentDropdownValueChanged);
        }
    }

    // Called when the currentDropdown value changes
    void OnCurrentDropdownValueChanged(int value)
    {
        if (Dropdown != null)
        {
            // Update the external Dropdown to match currentDropdown
            Dropdown.value = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Dropdown != null && currentDropdown != null)
        {
            // Check if the options list has changed
            if (!AreOptionsEqual(currentDropdown.options, Dropdown.options))
            {
                // Update options and reset value
                currentDropdown.options = new List<TMP_Dropdown.OptionData>(Dropdown.options);

                // Set value to -1 (reset state) and then to 0
                currentDropdown.value = -1;
                optionsUpdated = true; // Flag to update value to 0 in the next frame
            }

            // If options were updated, set value to 0
            if (optionsUpdated)
            {
                currentDropdown.value = 0;
                optionsUpdated = false;
            }
        }
    }

    // Helper method to check if two lists of options are equal
    private bool AreOptionsEqual(List<TMP_Dropdown.OptionData> options1, List<TMP_Dropdown.OptionData> options2)
    {
        if (options1.Count != options2.Count) return false;

        for (int i = 0; i < options1.Count; i++)
        {
            if (options1[i].text != options2[i].text)
            {
                return false;
            }
        }

        return true;
    }
}
