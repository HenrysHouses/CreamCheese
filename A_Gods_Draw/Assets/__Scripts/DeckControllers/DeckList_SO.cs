using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: save cards in json/playprefs

[CreateAssetMenu(menuName = "Card/DeckList"), System.Serializable]
public class DeckList_SO : ScriptableObject
{
    [SerializeField]
    public List<Card_SO> Deck;
}