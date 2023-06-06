using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_LibraryHighlight : MonoBehaviour
{
    public GameObject highlight;
    [SerializeField] ReaderTarget CardInspector;
    IsHovering targetCollider;
    bool hasHighlight;
    bool hovering;

    void Start()
    {
        targetCollider = GetComponentInParent<IsHovering>();
    }

    private void Update()
    {
        bool hasCollider = false;
        if(targetCollider != null)
        {
            if(targetCollider.isHovering)
            {
                hasCollider = true;

            }
        }
        if (CardInspector.isBeingInspected || hovering || hasCollider)
        {
            highlight.SetActive(true);
            hasHighlight = true;
        }
        else
        {
            highlight.SetActive(false);
            hasHighlight = false;
        }
    }

    private void OnMouseEnter()
    {
        hovering = true;
    }

    private void OnMouseExit()
    {
        hovering = false;
    }
}
