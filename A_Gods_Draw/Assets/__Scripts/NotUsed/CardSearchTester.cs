/*
 * Written by:
 * Henrik
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A_Gods_Draw/CardSearchTester")]
public class CardSearchTester : ScriptableObject
{
    public string[] cardNames;

    public List<Card_SO> foundCards;

    public void searchCards()
    {
        List<Card_SO> found = CardSearch.Search<Card_SO>(cardNames);
        foundCards = found;
    }
}
