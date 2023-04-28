using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_LibraryHighlight : MonoBehaviour
{
    public GameObject highlight;
    [SerializeField] ReaderTarget CardInspector;
    bool hasHighlight;

    private void Update()
    {
        if (CardInspector.isBeingInspected)
        {
            highlight.SetActive(true);
            hasHighlight = true;
        }
    }

    private void OnMouseEnter()
    {
        if (!hasHighlight)
        {
            highlight.SetActive(true);
            hasHighlight = true;
        }
    }

    

    private void OnMouseExit()
    {
        if(hasHighlight)
        {
            highlight.SetActive(false);
            hasHighlight = false;
        }
    }
}
