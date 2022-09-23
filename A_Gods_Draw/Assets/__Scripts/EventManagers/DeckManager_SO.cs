/*
 * Written by:
 * Henrik
 *
 * Script Purpose:
 * Keeping track of how many and which cards are in the player's Deck, Library, Hand, and Discard.
 * Requests animations for card draw, discard, and deck shuffles.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>Keeps track of how many and which cards are in the player's Deck, Library, Hand, and Discard. Requests animations for card draw, discard, and deck shuffles.</summary>
[CreateAssetMenu(menuName = "Events/DeckManager")]
public class DeckManager_SO : ScriptableObject
{
    // [SerializeField] TurnManager turnManager;
    [SerializeField, Tooltip("needed to request animations")] 
    AnimationManager_SO animationManager;
    
    [SerializeField, Tooltip("Prefab used in animations for cards")] 
    GameObject CardAnimationPrefab;
    
    [SerializeField, Tooltip("Cards the player has obtained")] 
    DeckList_SO deckList;
    
    [SerializeField, Tooltip("Cards the player can draw")] 
    List<Card_SO> pLibrary;
    
    [SerializeField, Tooltip("Cards the player has discarded")] 
    List<Card_SO> pDiscard;
    
    [SerializeField, Tooltip("Cards in the player's current hand")] 
    List<Card_SO> pHand;

    // ? Not sure if these are going to be used
    // Events for when the card lists change. 
    [System.NonSerialized]
    public UnityEvent deckListChangeEvent; 
    [System.NonSerialized]
    public UnityEvent pLibraryChangeEvent;
    [System.NonSerialized]
    public UnityEvent pDiscardChangeEvent;
    [System.NonSerialized]
    public UnityEvent pHandChangeEvent;

    // ! unused
    // public void SetTurnManager(TurnManager manager)
    // {
    //     turnManager = manager;
    // }

    // Load the deck list from Assets/Resources/DeckLists
    void OnValidate()
    {
        if (deckList == null)
            deckList = Resources.Load<DeckList_SO>("DeckLists/DeckList");
    }

    // Setup
    void OnEnable()
    {
        if (deckListChangeEvent == null)
            deckListChangeEvent = new UnityEvent();

        pLibrary = new List<Card_SO>();
        for (int i = 0; i < deckList.Deck.Count; i++)
        {
            pLibrary.Add(deckList.Deck[i]);
        }

        if(pLibraryChangeEvent == null)
            pLibraryChangeEvent = new UnityEvent();
        
        pDiscard = new List<Card_SO>();
        if(pDiscardChangeEvent == null)
            pDiscardChangeEvent = new UnityEvent();
        
        pHand = new List<Card_SO>();
        if (pHandChangeEvent == null)
            pHandChangeEvent = new UnityEvent();
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
        deckList.Deck.Add(card);
        // ? change events may not be used
        deckListChangeEvent.Invoke();
    }

    /// <summary>Removes a card from the player deck list</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromDeck(Card_SO card)
    {
        deckList.Deck.Remove(card);
        // ? change events may not be used
        deckListChangeEvent.Invoke();
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
        // ? change events may not be used
        pLibraryChangeEvent.Invoke();
    }

    /// <summary>Removes a card from the player library</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromLibrary(Card_SO card)
    {
        pLibrary.Remove(card);
        // ? change events may not be used
        pLibraryChangeEvent.Invoke();
    }

    /// <summary>Adds a card to the player discard</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void addCardToDiscard(Card_SO card)
    {
        pDiscard.Add(card);
        // ? change events may not be used
        pDiscardChangeEvent.Invoke();
    }

    /// <summary>Removes a card from the player discard</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromDiscard(Card_SO card)
    {
        pDiscard.Remove(card);
        // ? change events may not be used
        pDiscardChangeEvent.Invoke();
    }
    
    /// <summary>Adds a card to the player hand</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void addCardToHand(Card_SO card)
    {
        pHand.Add(card);
        // ? change events may not be used
        pHandChangeEvent.Invoke();
    }

    /// <summary>Removes a card from the player hand</summary>
    /// <param name="card">The scriptable object for the desired card</param>
    public void removeCardFromHand(Card_SO card)
    {
        pHand.Remove(card);
        // ? change events may not be used
        pHandChangeEvent.Invoke();
    }

    /// <summary>Move the top card/s of the player library to the player hand. Trigger card draw animations</summary>
    /// <param name="amount">The amount of cards to draw</param>
    /// ! <returns></returns> // Missing return summary
    public void drawCard(int amount)
    {
        if (pLibrary.Count < amount) // if there is no cards in library to draw, shuffle the discard into the library and return
        {
            shuffleDiscard();
        }

        // Instantiate the object that will be animated.
        GameObject[] cards = new GameObject[amount]; 
        // Animation setup, Sets the event to spawn the card when animation is completed, requests the animation
        PathAnimatorController.pathAnimation[] animations = new PathAnimatorController.pathAnimation[amount];
            
        for (int i = 0; i < amount; i++) 
        {
            // adds the top card to player hand
            pHand.Add(pLibrary[0]); 
            cards[i] = Instantiate(CardAnimationPrefab);

            Card_Loader _Loader = cards[i].GetComponentInChildren<Card_Loader>();
            _Loader.Set(pLibrary[0], null);
            pLibrary.Remove(pLibrary[0]);


            animations[i] = new PathAnimatorController.pathAnimation();
            animations[i].CompletionTrigger.AddListener(_Loader.moveCardToHand);

            //Just to make them clickable
            cards[i].transform.position = new Vector3(20, 0, 0);
            //card.transform.rotation = Quaternion.Euler(-20 + i * 10, 90, 0);
        }
        animationManager.requestAnimation("Library-Hand", cards, 0, 0.25f, animations);
        
        // ? change events may not be used
        pLibraryChangeEvent.Invoke();
        pHandChangeEvent.Invoke();
        return;
    }

    /// <summary>Moves all cards currently in player hand to player discard. Trigger discard animations</summary>
    public void discardAll()
    {
        // Moves cards in hand to discard
        GameObject[] cards = new GameObject[pHand.Count];
        for (int i = 0; i < pHand.Count; i++)
        {
            // preps the discard animations
            GameObject _card = Instantiate(CardAnimationPrefab);
            _card.GetComponentInChildren<Card_Loader>().Set(pHand[i], null);
            cards[i] = _card;

            pDiscard.Add(pHand[i]);
        }
        // requests animations for all discarded cards
        animationManager.requestAnimation("Hand-Discard", cards, 0, 0.25f);

        pHand.Clear();
        // ? change events may not be used
        pHandChangeEvent.Invoke();
        pDiscardChangeEvent.Invoke();
    }

    /// <summary>Moves a card currently in player hand to player discard. Trigger discard animation</summary>
    public void discardCard(Card_SO card)
    {
        if(pHand.Contains(card))
        {
            // preps the discard animation
            GameObject _card = Instantiate(CardAnimationPrefab);
            _card.GetComponentInChildren<Card_Loader>().Set(card, null);
            animationManager.requestAnimation("Hand-Discard", _card);

            pDiscard.Add(card);
            pHand.Remove(card);
        }
        // ? change events may not be used
        pHandChangeEvent.Invoke();
        pDiscardChangeEvent.Invoke();
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
        // ? change events may not be used
        pLibraryChangeEvent.Invoke();
    }
    
    /// <summary>Moves all cards from discard to the library, Then shuffle the library</summary>
    public void shuffleDiscard()
    {
        // move discard to library, prep for animations
        GameObject[] cards = new GameObject[pDiscard.Count];
        for (int i = 0; i < pDiscard.Count; i++)
        {
            pLibrary.Add(pDiscard[i]);
            cards[i] = Instantiate(CardAnimationPrefab);
            cards[i].GetComponentInChildren<Card_Loader>().Set(pDiscard[i], null);
        }
        // Request discard to library animations
        animationManager.requestAnimation("ShuffleDiscard", cards, 0, 0.18f);
        pDiscard.Clear();
        // ? change events may not be used
        pDiscardChangeEvent.Invoke();
        shuffleLibrary();
    }

    /// <returns>Current cards in the player's hand</returns>
    public List<Card_SO> GetCurrentHand()
    {
        return pHand;
    }

    public void wtf() { Debug.Log("aaaa"); }
}