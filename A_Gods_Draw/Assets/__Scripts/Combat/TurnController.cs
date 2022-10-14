using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] BoardStateController boardState;
    [SerializeField] PlayerTracker player;
    [SerializeField] DeckManager_SO deckManager;

    void Awake()
    {
        deckManager.SetCurrentDeck(player.Deck);
        deckManager.clear();
    }

    private void Update() 
    {
        if(boardState.isEncounterInstantiated)
        {
            
        }   
    }
}
