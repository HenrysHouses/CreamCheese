using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardElement : MonoBehaviour
{
    protected static bool isInCombat;
    public bool clickable;

    public static void EnterCombat() { isInCombat = true; }
    public static void ExitCombat() { isInCombat = false; }

    public virtual bool OnClick()
    {
        return clickable && !isInCombat; 
    }

}
