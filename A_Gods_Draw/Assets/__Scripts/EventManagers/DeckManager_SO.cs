/*
 * Written by:
 * Henrik
 * 
 * Modified by:
 * Charlie
 *
 * Script Purpose:
 * Keeping track of how many and which cards are in the player's Deck, Library, Hand, and Discard.
 * Requests animations for card draw, discard, and deck shuffles.
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

/// <summary>Keeps track of how many and which cards are in the player's Deck, Library, Hand, and Discard. Requests animations for card draw, discard, and deck shuffles.</summary>
[CreateAssetMenu(menuName = "Events/DeckManager")]
public class DeckManager_SO : ScriptableObject
{
    [SerializeField, Tooltip("Prefab used in animations for cards")] 
    GameObject CardAnimationPrefab;
    
    [SerializeField, Tooltip("Cards the player has obtained")] 
    DeckList_SO deckList;
    public DeckList_SO getDeck => deckList;
    [SerializeField] DeckList_SO starterDeck;
    public DeckList_SO getStarterDeck => starterDeck;
    
    [SerializeField, Tooltip("Cards the player can draw")] 
    List<Card_SO> pLibrary;
    
    [SerializeField, Tooltip("Cards the player has discarded")] 
    List<Card_SO> pDiscard;
    public int GetDiscardCount() => pDiscard.Count;
    
    [SerializeField, Tooltip("Cards in the player's current hand")] 
    List<Card_SO> pHand;

    [SerializeField] EventReference Draw_SFX;
    [SerializeField] EventReference Discard_SFX;
    [SerializeField] EventReference Shuffle_SFX;

    // Load the deck list from Assets/Resources/DeckLists
    void OnValidate()
    {
        if (deckList == null)
        {
            deckList = Resources.Load<DeckList_SO>("DeckLists/DeckList");
        }
            
    }

    // Setup
    void OnEnable()
    {
        pLibrary = new List<Card_SO>();
        for (int i = 0; i < deckList.Deck.Count; i++)
        {
            pLibrary.Add(deckList.Deck[i]);
        }

        pDiscard = new List<Card_SO>();
        pHand = new List<Card_SO>();
    }

    // Clears the lists used for game play
    void OnDisable()
    {
        clear();
    }

    /// <summary>Adds a card to the player deck list</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void addCardToDeck(Card_SO card)
    {
        Debug.Log("add card: " + card);
        deckList.Deck.Add(card);
        SavingDeck();
    }

    /// <summary>Removes a card from the player deck list</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromDeck(Card_SO card)
    {
        deckList.Deck.Remove(card);
        SavingDeck();
    }

    /// <summary>Sets player library to be equal to the deck list, clears player hand and discard.</summary>
    public void reset()
    {
        pLibrary.Clear();
        for (int i = 0; i < deckList.Deck.Count; i++)
        {
            pLibrary.Add(deckList.Deck[i]);
        }
        pHand.Clear();
        pDiscard.Clear();
    }

    /// <summary>Clear all card lists the player uses for combat</summary>
    public void clear()
    {
        pLibrary.Clear();
        pDiscard.Clear();
        pHand.Clear();
    }

    /// <summary>Adds a card to the player library</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void addCardToLibrary(Card_SO card)
    {
        pLibrary.Add(card);
    }

    /// <summary>Removes a card from the player library</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromLibrary(Card_SO card)
    {
        pLibrary.Remove(card);
    }

    /// <summary>Adds a card to the player discard</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void addCardToDiscard(Card_SO card)
    {
        pDiscard.Add(card);
    }

    /// <summary>Removes a card from the player discard</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromDiscard(Card_SO card)
    {
        pDiscard.Remove(card);
    }
    
    /// <summary>Adds a card to the player hand</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void addCardToHand(Card_SO card)
    {
        pHand.Add(card);
    }

    /// <summary>Removes a card from the player hand</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromHand(Card_SO card)
    {
        pHand.Remove(card);
    }

    /// <summary>Move the top card/s of the player library to the player hand. Trigger card draw animations</summary>
    /// <param name="amount">The amount of cards to draw</param>
    /// <returns>List of all animation OnCompletionEvents</returns>
    public CardPathAnim[] drawCard(int amount, float delay)
    {
        if (pLibrary.Count < amount) // if there is no cards in library to draw, shuffle the discard into the library and return
        {
            Debug.Log("cant draw cards, shuffle discard first");
            return null;
            // shuffleDiscard();
        }

        // Instantiate the object that will be animated.
        GameObject[] cards = new GameObject[amount]; 
        // Animation setup, Sets the event to spawn the card when animation is completed, requests the animation
        CardPathAnim[] animations = new CardPathAnim[amount];
        for (int i = 0; i < amount; i++) 
        {
            // adds the top card to player hand
            pHand.Add(pLibrary[0]); 
            cards[i] = Instantiate(CardAnimationPrefab);

            Card_Loader _Loader = cards[i].GetComponentInChildren<Card_Loader>();
            _Loader.shouldAddComponent = false;
            _Loader.Set(pLibrary[0]);
            pLibrary.Remove(pLibrary[0]);
            animations[i] = new CardPathAnim(_Loader.GetCardSO, Draw_SFX, cards[i]);

            // ! not clear purpose of this code
            // if (i == amount - 1 && mngr != null)
            // {
            //     animations[i].CompletionTrigger.AddListener
            //         (mngr.HandFull);
            // }

            // Debug.Log("Sent card: " + cards[i].name + " with animation: " + animations[i].index + ", number " + i);

            //Just to make them clickable
            //cards[i].transform.position = new Vector3(20, 0, 0);
            //card.transform.rotation = Quaternion.Euler(-20 + i * 10, 90, 0);
        }
        AnimationEventManager.getInstance.requestAnimation("Library-Hand", cards, delay, animations);
        return animations;
    }

    /// <summary>Moves all cards currently in player hand to player discard. Trigger discard animations</summary>
    /// <returns>List of all animation OnCompletionEvents</returns>
    public CardPathAnim[] discardAll(float delay)
    {
        if(pHand.Count == 0)
        {
            return null;
        }
        // Moves cards in hand to discard
        GameObject[] cards = new GameObject[pHand.Count];

        CardPathAnim[] animations = new CardPathAnim[pHand.Count];

        for (int i = 0; i < pHand.Count; i++)
        {
            // preps the discard animations
            GameObject _card = Instantiate(CardAnimationPrefab);
            Card_Loader _Loader = _card.GetComponentInChildren<Card_Loader>();
            _Loader.shouldAddComponent = false;
            _Loader.Set(pHand[i]);

            cards[i] = _card;

            pDiscard.Add(pHand[i]);

            animations[i] = new CardPathAnim(_Loader.GetCardSO, Discard_SFX, _card);
        }

        // requests animations for all discarded cards
        AnimationEventManager.getInstance.requestAnimation("Hand-Discard", cards, delay, animations);

        pHand.Clear();
        return animations;
    }

    public void discardAll(float delay, Card_SO exceptFor)
    {
        // Moves cards in hand to discard
        List<GameObject> cards = new();

        CardPathAnim[] animations = new CardPathAnim[pHand.Count];

        for (int i = 0; i < pHand.Count; i++)
        {
            Card_Loader _Loader = null;
            GameObject _card = null;
            if (pHand[i] != exceptFor)
            {
                Debug.LogWarning("ExceptFor variable may cause issues with animations");
                // preps the discard animations
                _card = Instantiate(CardAnimationPrefab);
                _Loader = _card.GetComponentInChildren<Card_Loader>();
                _Loader.shouldAddComponent = false;
                _Loader.Set(pHand[i]);
                cards.Add(_card);
                pDiscard.Add(pHand[i]);
            }
            animations[i] = new CardPathAnim(_Loader.GetCardSO, Discard_SFX, _card);
        }
        // requests animations for all discarded cards

        if (cards.Count == 0)
        {
            // if (mngr != null)
            // {
            //     mngr.FinishedAnimations();
            // }
            return;
        }

        // if (mngr != null)
            // animations[cards.Count - 1].CompletionTrigger.AddListener(mngr.FinishedAnimations);
        AnimationEventManager.getInstance.requestAnimation("Hand-Discard", cards.ToArray(), delay, animations);


        pHand.Clear();
    }

    /// <summary>Moves a card currently in player hand to player discard. Trigger discard animation</summary>
    public CardPathAnim discardCard(Card_SO card)
    {
        if (pHand.Contains(card))
        {
            // preps the discard animation
            GameObject _card = Instantiate(CardAnimationPrefab);
            Card_Loader _Loader = _card.GetComponentInChildren<Card_Loader>();
            _Loader.shouldAddComponent = false;
            _Loader.Set(card);
            CardPathAnim anim = new CardPathAnim(card, Discard_SFX, _card);
            AnimationEventManager.getInstance.requestAnimation("Hand-Discard", _card, 0, anim);

            pDiscard.Add(card);
            pHand.Remove(card);

            return anim;
        }
        return null;
    }

    /// <summary>Completely randomizes the order of the library</summary>
    public void shuffleLibrary()
    {
        // prep for randomization
        List<Card_SO> libraryCopy = new List<Card_SO>();
        for (int i = 0; i < pLibrary.Count; i++)
        {
            libraryCopy.Add(pLibrary[i]);
        }
        pLibrary.Clear();

        // iterate through library copy and add them into the player library at random 
        for (int i = 0; i < libraryCopy.Count; i++)
        {
            int rnd = Random.Range(0, libraryCopy.Count);
            while(libraryCopy[rnd] == null)
            {
                rnd = Random.Range(0, libraryCopy.Count);
            }
            pLibrary.Add(libraryCopy[rnd]);
            libraryCopy[rnd] = null;
        }
    }
    
    /// <summary>Moves all cards from discard to the library, Then shuffle the library</summary>
    /// <returns>List of all animation OnCompletionEvents</returns>
    public CardPathAnim[] shuffleDiscard(float delay)
    {
        // move discard to library, prep for animations
        GameObject[] cards = new GameObject[pDiscard.Count];
        CardPathAnim[] animations = new CardPathAnim[pDiscard.Count];
        
        if(pDiscard.Count == 0)
            Debug.LogWarning("There was no cards in discard to shuffle into the library");
        
        for (int i = 0; i < pDiscard.Count; i++)
        {
            pLibrary.Add(pDiscard[i]);
            cards[i] = Instantiate(CardAnimationPrefab);
            Card_Loader _Loader = cards[i].GetComponentInChildren<Card_Loader>();
            _Loader.shouldAddComponent = false;
            _Loader.Set(pDiscard[i]);
            animations[i] = new CardPathAnim(_Loader.GetCardSO, Shuffle_SFX, cards[i]);
        }
        // Request discard to library animations
        AnimationEventManager.getInstance.requestAnimation("ShuffleDiscard", cards, delay, animations);
        pDiscard.Clear();
        shuffleLibrary();
        return animations;
    }

    /// <returns>Current cards in the player's hand</returns>
    public Card_SO[] GetHandSO()
    {
        return pHand.ToArray();
    }

    public void SetCurrentDeck(DeckList_SO deckList_)
    {
        deckList = deckList_;
    }

    public void SavingDeck()
    {
        var settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        var json = JsonUtility.ToJson(deckList).ToString();

        PlayerPrefs.SetString("Deck", json);
        PlayerPrefs.Save();

        Debug.Log("deck list has been saved");
    }

    public void LoadDeck()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string deckJson = PlayerPrefs.GetString("Deck");
        
        DeckList_SO loadedDeck = ScriptableObject.CreateInstance<DeckList_SO>();
        JsonUtility.FromJsonOverwrite(deckJson, loadedDeck);

        Debug.Log(loadedDeck);
        
        if(loadedDeck.Deck == null)
        {
            Debug.LogWarning("The list in the Loaded deck is null");
            deckList.Deck = starterDeck.Deck;
            SavingDeck();
            return;
        }

        if(loadedDeck.Deck.Count < 1) // ! probably never going to happen
        {
            Debug.LogWarning("Loaded deck is length of 0");
            deckList.Deck = starterDeck.Deck;
            return;
        }

        deckList.Deck = loadedDeck.Deck;
        Debug.Log(loadedDeck.Deck[0].cardName);

        Debug.Log("deck has been loaded from the save");
    }
}