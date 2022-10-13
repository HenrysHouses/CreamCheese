using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{ public string[] tutorialText;
    bool[] tutorialChecks;
    public TMP_Text inputText;

    // Start is called before the first frame update
    void Start()
    {
        inputText.text = tutorialText[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
