/* 
 * Written by 
 * Henrik
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerTracker")]
public class PlayerTracker : ScriptableObject
{
    public int Health;
    public DeckList_SO Deck;

    // player's runes here

    public void setDeck(DeckList_SO deck)
    {

        List<Card_SO> newDeck = new List<Card_SO>(deck.Deck.Count);
        for (int i = 0; i < newDeck.Count; i++)
        {
            newDeck[i] = deck.Deck[i];
        }

        Deck.Deck = newDeck;
    }
}