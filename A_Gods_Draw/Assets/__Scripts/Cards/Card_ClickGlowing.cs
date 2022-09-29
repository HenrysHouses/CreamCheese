//charlie

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Card_ClickGlowing : MonoBehaviour
{
    public GameObject[] glowBorders;
    public Transform parent;
    bool isCreated;
    public static Component currentlySelected;

    private void OnMouseOver()
    {
        if (!isCreated)
        {
            currentlySelected = this;
            
            glowBorders[0].SetActive(true);
            isCreated = true;

        }
    }

    private void OnMouseExit()
    {
        if (isCreated)
        {
            glowBorders[0].SetActive(false);
            isCreated = false;
        }
        
    }
}
