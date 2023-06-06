using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHovering : MonoBehaviour
{
    public bool isHovering {get; private set;}
    void OnMouseEnter()
    {
        isHovering = true;
    }

    void OnMouseExit()
    {
        isHovering = false;
    }
}
