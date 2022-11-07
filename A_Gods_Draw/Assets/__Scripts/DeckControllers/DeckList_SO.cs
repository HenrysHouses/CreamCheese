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
    }
}

[System.Serializable]
public class DeckListData
{
    public DeckListData(CardQuantity[] cardQuantities)
    {
        List<string> SearchNames = new List<string>();
        for (int i = 0; i < cardQuantities.Length; i++)
        {
            SearchNames.Add(cardQuantities[i].CardName);
        }

        deckListData = CardSearch.Search<Card_SO>(SearchNames.ToArray());

        for (int i = 0; i < cardQuantities.Length; i++)
        {
            int n = 1;

            while(n < cardQuantities[i].Quantity)
            {
                string[] name = {cardQuantities[i].CardName};
                List<Card_SO> card = CardSearch.Search<Card_SO>(name);
                deckListData.Add(card[0]);
                n++;
                Debug.Log(n);
            }
        }

        Debug.Log(deckListData.Count);
    }

    public DeckListData(){}

    public List<Card_SO> deckListData;
    
    public CardQuantityContainer GetDeckCardNames()
    {
        CardQuantityContainer container = new CardQuantityContainer();



        container.Cards = new CardQuantity[GetNumbOfUniqueCards()];
        Card_SO[] UniqueCards = GetUniqueCards();

        for (int i = 0; i < container.Cards.Length; i++)
        {
            string card = UniqueCards[i].cardName;
            container.Cards[i].CardName = card;
            container.Cards[i].Quantity = GetQuantityOfCard(card);
        }
        return container;
    } 

    Card_SO[] GetUniqueCards()
    {
        List<Card_SO> unique = new List<Card_SO>();

        for (int i = 0; i < deckListData.Count; i++)
        {
            if(!unique.Contains(deckListData[i]))
            {
                unique.Add(deckListData[i]);
            }
        }
        return unique.ToArray();
    }

    int GetNumbOfUniqueCards()
    {
        List<Card_SO> unique = new List<Card_SO>();

        for (int i = 0; i < deckListData.Count; i++)
        {
            if(!unique.Contains(deckListData[i]))
            {
                unique.Add(deckListData[i]);
            }
        }
        return unique.Count;
    }

    int GetQuantityOfCard(string cardName)
    {
        int n = 0;

        for (int i = 0; i < deckListData.Count; i++)
        {
            if(deckListData[i].cardName == cardName)
                n++;
        }

        return n;
    }
}

[System.Serializable]
public struct CardQuantityContainer
{
    public CardQuantity[] Cards;
}

[System.Serializable]
public struct CardQuantity
{
    public string CardName;
    public int Quantity;
}