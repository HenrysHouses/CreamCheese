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
        List<Card_SO> newDeck = new List<Card_SO>();
        for (int i = 0; i < deck.Deck.Count; i++)
        {
            newDeck.Add(deck.Deck[i]);
        }

        Deck.Deck = newDeck;
    }
}