using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: save cards in json/playprefs

[CreateAssetMenu(menuName = "Card/DeckList"), System.Serializable]
public class DeckList_SO : ScriptableObject
{
    [SerializeField]
    public DeckListData deckData;

    public List<Card_SO> GetDeck()
    {
        return deckData.deckListData;
    }

    public void SetDeck(List<Card_SO> deckList)
    {
        deckData.deckListData = deckList;
        Debug.Log(deckList.Count);
    }
}

[System.Serializable]
public class DeckListData
{
    public List<Card_SO> deckListData;
}