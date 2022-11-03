using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Card/DeckList")]
public class DeckList_SO : ScriptableObject
{
    [SerializeField]
    public List<Card_SO> Deck;
}