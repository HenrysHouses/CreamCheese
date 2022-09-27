//charlie

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Card_ClickGlowing : MonoBehaviour
{
    public GameObject glowBorder;
    public Transform parent;
    bool isCreated;
    public static Component currentlySelected;


    private void OnMouseOver()
    {
        if (!isCreated)
        {
            currentlySelected = this;
            
            glowBorder.SetActive(true);
            isCreated = true;

        }
    }

    private void OnMouseExit()
    {
        if (isCreated)
        {
            glowBorder.SetActive(false);
            isCreated = false;
        }
        
    }

    /*public void Deselect()
    {
        glowBorder.SetActive(false);
        isCreated = false;
    }*/
}
