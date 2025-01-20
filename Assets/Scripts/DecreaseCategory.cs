using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DecreaseCategory : MonoBehaviour
{
    public TMP_Text TMP_Text;
    public Button IncreaseButton;
    public LoadText loadText;

    // Start is called before the first frame update
    void Start()
    {
        IncreaseButton.onClick.AddListener(Increase);
    }
    void Increase()
    {
        int currentCategory = int.Parse(TMP_Text.text);
        if(currentCategory == 1)
            return;
        TMP_Text.text = (currentCategory - 1).ToString();
        loadText.LoadTextValue(loadText.dropdown.value);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
