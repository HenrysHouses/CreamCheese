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
    public GameObject ui; //the canvas object on the card
    public bool isMade;


    /*private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ui.SetActive(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ui.SetActive(false);
        }
    }*/

    private void OnMouseOver()
    {
        if (!isMade)
        {
            ui.SetActive(true);
            isMade = true;
        }
    }

    private void OnMouseExit()
    {
        if (isMade)
        {
            ui.SetActive(false);
            isMade = false;
        }
    }

}
