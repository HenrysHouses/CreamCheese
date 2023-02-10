/* 
 * Written by 
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/DeckList"), System.Serializable]
public class DeckList_SO : ScriptableObject
{
    [SerializeField]
    public DeckListData deckData;
    public List<CardPlayData> GetDeck() => deckData.deckListData;
}

/// <summary>Data container for the player's deck, Used for saving and loading</summary>
[System.Serializable]
public class DeckListData
{
    public List<CardPlayData> deckListData = new List<CardPlayData>();
    public int Count => deckListData.Count;

    // Constructor
    public DeckListData(CardQuantity[] cardQuantities)
    {
        // Adding all the instances of unique cards
        for (int i = 0; i < cardQuantities.Length; i++)
        {
            int n = 0;

            // While there is still more copies of a single card to add
            while(n < cardQuantities[i].Quantity)
            {
                string[] name = {cardQuantities[i].CardName};
                // Find spesific card
                List<Card_SO> card = CardSearch.Search<Card_SO>(name);
                
                // Add its scriptable Object, Experience and level
                CardPlayData data = new CardPlayData();
                data.CardType = card[0];
                data.Experience = cardQuantities[i].Levels[n];
                data.Experience.Level = cardQuantities[i].Levels[n].Level;
                // Insert into deck data
                deckListData.Add(data);
                n++;
            }
        }
    }
    
    public CardQuantityContainer GetDeckData()
    {
        CardQuantityContainer container = new CardQuantityContainer();

        container.Cards = new CardQuantity[GetNumbOfUniqueCards()];
        Card_SO[] UniqueCards = GetUniqueCards();

        for (int i = 0; i < container.Cards.Length; i++)
        {
            string card = UniqueCards[i].cardName;
            container.Cards[i].CardName = card;
            container.Cards[i].SetQuantity(GetQuantityOfCard(card));
        }

        for (int i = 0; i < container.Cards.Length; i++)
        {
            container.Cards[i].SetCardLevels(deckListData.ToArray());
        }

        return container;
    } 

    Card_SO[] GetUniqueCards()
    {
        List<Card_SO> unique = new List<Card_SO>();

        for (int i = 0; i < deckListData.Count; i++)
        {
            if(!unique.Contains(deckListData[i].CardType))
            {
                unique.Add(deckListData[i].CardType);
            }
        }
        return unique.ToArray();
    }

    int GetNumbOfUniqueCards()
    {
        List<Card_SO> unique = new List<Card_SO>();

        for (int i = 0; i < deckListData.Count; i++)
        {
            if(!unique.Contains(deckListData[i].CardType))
            {
                unique.Add(deckListData[i].CardType);
            }
        }
        return unique.Count;
    }

    int GetQuantityOfCard(string cardName)
    {
        int n = 0;

        for (int i = 0; i < deckListData.Count; i++)
        {
            if(deckListData[i].CardType.cardName == cardName)
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
    int _quantity;
    public int Quantity => _quantity;
    public CardExperience[] Levels;

    public void SetQuantity(int QuantityOfCard)
    {
        _quantity = QuantityOfCard;
        Levels = new CardExperience[_quantity];
    }

    public void SetCardLevels(CardPlayData[] Cards)
    {
        int index = 0;
        for (int i = 0; i < Cards.Length; i++)
        {
            if(Cards[i].CardType.cardName == CardName)
            {
                Cards[i].Experience.createNewUniqueID();
                Levels[index] = Cards[i].Experience;
                index++;
            }
        }
    }
}

[System.Serializable]
public struct CardPlayData
{
    public Card_SO CardType;
    [SerializeField]
    public CardExperience Experience;

    public void Clear()
    {
        CardType = null;
        Experience.XP = 0;
        Experience.Level = 0;
    }
}