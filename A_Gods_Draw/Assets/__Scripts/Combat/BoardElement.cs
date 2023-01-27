// Written by Javier Villegas
using UnityEngine;

/// <summary>
/// Class for any element on the board that could be interacted with
/// </summary>
public class BoardElement : InfoElement
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
