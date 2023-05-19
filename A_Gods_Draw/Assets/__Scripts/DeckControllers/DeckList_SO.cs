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
    public static DeckList_SO playerDeck;
    private void OnValidate() {
        if(playerDeck == null)
            playerDeck = Resources.Load<DeckList_SO>("DeckLists/DeckList");
    }

    public static void setPlayerDeck()
    {
        if(playerDeck == null)
            playerDeck = Resources.Load<DeckList_SO>("DeckLists/DeckList");
    }

    public static void playerObtainCard(Card_SO card_SO)
    {
        CardPlayData newCard = new CardPlayData(card_SO);
        CardExperience experience = newCard.Experience;
        experience.createGUID();
        newCard.Experience = experience;

        playerDeck.deckData.deckListData.Add(newCard);
        GameSaver.SaveData(playerDeck.deckData.GetDeckData());
    }

    public static void playerDeleteCard(CardPlayData Card)
    {
        playerDeck.deckData.deckListData.Remove(Card);
        GameSaver.SaveData(playerDeck.deckData.GetDeckData());
        //SavingDeck();
    }

    public static DeckList_SO getStarterDeck()
    {
        DeckList_SO starterDeck = Resources.Load<DeckList_SO>("DeckLists/StarterDeck");

        Debug.Log("Loaded starter deck!");

        return starterDeck;
    }

    public DeckList_SO Clone()
    {
        DeckList_SO list = ScriptableObject.CreateInstance<DeckList_SO>();
        list.deckData = deckData.Clone();
        return list;
    }
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
            while(n < cardQuantities[i].Levels.Length)
            {
                string[] name = {cardQuantities[i].CardName};
                // Find spesific card
                List<Card_SO> card = CardSearch.Search<Card_SO>(name);
                
                if(card.Count == 0)
                    throw new UnityException("Deck list contains an illegal card, \"" + cardQuantities[i].CardName + "\"");

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

    public Card_SO[] getCurrentCards()
    {
        Card_SO[] card_s = new Card_SO[deckListData.Count];

        for (int i = 0; i < deckListData.Count; i++)
        {
            card_s[i] = deckListData[i].CardType;
        }
        return card_s;
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

    public DeckListData Clone()
    {
        return (DeckListData)this.MemberwiseClone();
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
    public int _quantity {private set; get;}
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
                // Cards[i].Experience.createNewUniqueID();
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

    public CardPlayData(CardPlayData copy)
    {
        CardType = copy.CardType;

        CardExperience _XP = new CardExperience(copy.Experience.XP, copy.Experience.Level, copy.Experience.ID);
        Experience = _XP;
    }

    public CardPlayData(Card_SO Card)
    {
        Experience = new CardExperience();
        
        Experience.createGUID();

        CardType = Card;
    }

    // public void Clear()
    // {
    //     CardType = null;
    //     Experience.XP = 0;
    //     Experience.Level = 0;
    // }
}