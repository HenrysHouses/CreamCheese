/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

public class ShouldDestroyCardFromDeck : MonoBehaviour
{
    public void DestroyCardNextTimeInLibrary()
    {
        GameManager.instance.DestroyCardFromDeck();
    }
}
