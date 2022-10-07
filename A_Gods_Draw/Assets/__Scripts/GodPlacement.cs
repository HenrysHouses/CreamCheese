using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodPlacement : MonoBehaviour
{
    God_Behaviour god;
    public Image godArrow;

    public void SetGod(God_Behaviour god)
    {
        this.god = god;
    }

    public void GodShowArrow()
    {
        godArrow.enabled = true;
    }

    public void GodHideArrow()
    {
        godArrow.enabled = false;
    }
}
