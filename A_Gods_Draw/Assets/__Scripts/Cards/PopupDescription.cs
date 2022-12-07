//charlie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDescription : MonoBehaviour
{
    /// <summary>
    /// want to be able to make it big and be put to the middle of the screen
    /// or at least just make it bigger so that you are able to read
    /// should be next to the card you click on because that makes sense
    /// </summary>
    public GameObject ui, background; //the canvas object on the card
    public GameObject ui_lore, background_lore; //the lore 
    public bool isMade;
    public bool showBoth;


    private void OnMouseOver()
    {
        if (!isMade)
        {
            ui.SetActive(true);
            background.SetActive(true);
            isMade = true;

            if(showBoth)
            {
                ui_lore.SetActive(true);
                background_lore.SetActive(true);
            }
        }
    }

    private void OnMouseExit()
    {
        if (isMade)
        {
            ui.SetActive(false);
            background.SetActive(false);
            isMade = false;

            if(showBoth)
            {
                ui_lore.SetActive(false);
                background_lore.SetActive(false);
            }
        }
    }
    /*public void OnButtonClick()
    {
        if (isMade)
        {
            ui.SetActive(false);
            background.SetActive(false);
            Debug.Log("text is off");
        }
        else
        {
            ui.SetActive(true);
            background.SetActive(true);
            Debug.Log("text is on");
        }
    }*/
}
