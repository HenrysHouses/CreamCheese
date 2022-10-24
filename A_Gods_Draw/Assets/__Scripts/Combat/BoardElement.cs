using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardElement : MonoBehaviour
{
    static bool isInCombat;
    public bool clickable;

    public static void EnterCombat() { isInCombat = true; }
    public static void ExitCombat() { isInCombat = false; }

    private void OnMouseDown()
    {
        if (clickable && !isInCombat)
        {
            OnClick();
        }
    }

    protected abstract void OnClick();

}
