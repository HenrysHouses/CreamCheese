/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardDrawer_debugger : MonoBehaviour
{
    [SerializeField]
    private DeckManager_SO deckManager;
    [SerializeField]
    private Card_SO selectedCard;
    
#if UNITY_EDITOR
    [SerializeField]
    [HideInInspector]
    public int editor_drawAmount;
#endif

    public void addCardTest()
    {
        deckManager.addCardToDeck(selectedCard);
    }

    public void createLibrary()
    {
        deckManager.reset();
    }

    public void DrawACard(int amount)
    {
        deckManager.drawCard(amount, 0);
    }

    public void DiscardACard()
    {
        deckManager.discardCard(selectedCard);
    }

    public void DiscardHand()
    {
        deckManager.discardAll(0.25f);
    }

    public void Shuffle()
    {
        deckManager.shuffleLibrary();
    }

    public void Recycle()
    {
        deckManager.shuffleDiscard(0.18f);
    }
}