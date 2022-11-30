using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouldDestroyCardFromDeck : MonoBehaviour
{
    public void DestroyCardNextTimeInLibrary()
    {
        GameManager.instance.DestroyCardFromDeck();
    }
}
