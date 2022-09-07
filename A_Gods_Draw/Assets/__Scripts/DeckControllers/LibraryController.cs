/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    [SerializeField]
    DeckManager_SO deckManager;

    public void spawnDrawnCard()
    {

    }

    void OnEnable()
    {
        deckManager.reset();
        deckManager.drawCard(5);
    }

    void OnDisable()
    {
        deckManager.clear();
    }
}
