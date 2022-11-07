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
    [HideInInspector] public List<int> HealthChanges = new List<int>();
    public DeckList_SO CurrentDeck;

    // player's runes here

    public void UpdateHealth(int difference)
    {
        Health += difference;
        HealthChanges.Add(difference);
    }

    public void setDeck(DeckListData deckData)
    {
        List<Card_SO> newDeck = new List<Card_SO>();
        for (int i = 0; i < deckData.deckListData.Count; i++)
        {
            newDeck.Add(deckData.deckListData[i]);
        }

        CurrentDeck.SetDeck(newDeck);
    }
}