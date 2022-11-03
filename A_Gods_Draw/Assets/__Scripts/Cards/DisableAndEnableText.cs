//! charlie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAndEnableText : MonoBehaviour
{
    public GameObject ui, background; //the canvas object on the card
    public bool isMade;

    public void OnButtonClick()
    {
        if(isMade)
        {
            ui.SetActive(false);
            background.SetActive(false);
            //Debug.Log("text is off");
        }
        else
        {
            ui.SetActive(true);
            background.SetActive(true);
            //Debug.Log("text is on");
        }
    }
}
