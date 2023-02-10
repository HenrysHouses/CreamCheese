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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

/// <summary>Keeps track of how many and which cards are in the player's Deck, Library, Hand, and Discard. Requests animations for card draw, discard, and deck shuffles.</summary>
[CreateAssetMenu(menuName = "Events/DeckManager")]
public class DeckManager_SO : ScriptableObject
{
    [SerializeField, Tooltip("Prefab used in animations for cards")]
    GameObject CardDrawAnimationPrefab;
    [SerializeField, Tooltip("Prefab used in animations for cards")]
    GameObject CardShuffleAnimationPrefab;

    [SerializeField, Tooltip("Cards the player has obtained")]
    DeckList_SO deckList;
    public DeckList_SO getDeck => deckList;
    static DeckList_SO starterDeck;
    public UnityEvent OnLibraryChange;
    public UnityEvent OnDiscardChange;
    public UnityEvent OnShuffleDiscard;


    public static DeckList_SO getStarterDeck()
    {

        if(starterDeck != null)
        {
            return starterDeck;
        }
        
        starterDeck = Resources.Load<DeckList_SO>("DeckLists/StarterDeck");

        Debug.Log("Loaded starter deck!");

        return starterDeck;
    }

    [SerializeField, Tooltip("Cards the player can draw")] 
    List<CardPlayData> pLibrary;
    public int GetLibraryCount() => pLibrary.Count;
    
    [SerializeField, Tooltip("Cards the player has discarded")] 
    List<CardPlayData> pDiscard;
    public int GetDiscardCount() => pDiscard.Count;
    
    [SerializeField, Tooltip("Cards in the player's current hand")] 
    List<CardPlayData> pHand;

    public int GetAllCurrentPlayingCards() => pDiscard.Count + pLibrary.Count + pHand.Count;

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

    private void Awake()
    {
        OnLibraryChange = new UnityEvent();
        OnDiscardChange = new UnityEvent();
    }

    // Setup
    void OnEnable()
    {
        pLibrary = new List<CardPlayData>();
        for (int i = 0; i < deckList.deckData.Count; i++)
        {
            pLibrary.Add(deckList.deckData.deckListData[i]);
        }

        pDiscard = new List<CardPlayData>();
        pHand = new List<CardPlayData>();
    }

    // Clears the lists used for game play
    void OnDisable()
    {
        clear();
    }

    /// <summary>Adds a card to the player deck list</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void addCardToDeck(CardPlayData card)
    {
        Debug.Log("add card: " + card);
        deckList.GetDeck().Add(card);
        GameSaver.SaveData(deckList.deckData.GetDeckData());
        //SavingDeck();
    }

    /// <summary>Removes a card from the player deck list</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void removeCardFromDeck(CardPlayData card)
    {
        deckList.GetDeck().Remove(card);
        GameSaver.SaveData(deckList.deckData.GetDeckData());
        //SavingDeck();
    }

    /// <summary>Sets player library to be equal to the deck list, clears player hand and discard.</summary>
    public void reset()
    {
        pLibrary.Clear();
        for (int i = 0; i < deckList.GetDeck().Count; i++)
        {
            pLibrary.Add(deckList.GetDeck()[i]);
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
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void addCardToLibrary(CardPlayData card)
    {
        pLibrary.Add(card);
    }

    /// <summary>Removes a card from the player library</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void removeCardFromLibrary(CardPlayData card)
    {
        pLibrary.Remove(card);
    }

    /// <summary>Adds a card to the player discard</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void addCardToDiscard(CardPlayData card)
    {
        pDiscard.Add(card);
    }

    /// <summary>Removes a card from the player discard</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void removeCardFromDiscard(CardPlayData card)
    {
        pDiscard.Remove(card);
    }
    
    /// <summary>Adds a card to the player hand</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void addCardToHand(CardPlayData card)
    {
        pHand.Add(card);
    }

    /// <summary>Removes a card from the player hand</summary>
    /// <param name="card">Data container for the scriptable object and the card's experience and current level</param>
    public void removeCardFromHand(CardPlayData card)
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
            // Debug.Log("cant draw cards, shuffle discard first");
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
            cards[i] = Instantiate(CardDrawAnimationPrefab);
            cards[i].transform.position = Vector3.zero;  

            Card_Loader _Loader = cards[i].GetComponentInChildren<Card_Loader>();
            _Loader.addComponentAutomatically = false;
            _Loader.Set(pLibrary[0]);
            pLibrary.Remove(pLibrary[0]);
            animations[i] = new CardPathAnim(_Loader._card, Draw_SFX, cards[i], GodDialogueTrigger.Draw);
        }
        OnLibraryChange?.Invoke();
        AnimationEventManager.getInstance.requestAnimation("Library-Hand", cards, delay, animations);
        return animations;
    }

    /// <summary>Moves a card currently in player hand to player discard. Trigger discard animation</summary>
    public CardPathAnim discardCard(CardPlayData card, List<CardPlayData> exhausted)
    {
        if (pHand.Contains(card))
        {
            // preps the discard animation
            GameObject _card = Instantiate(CardDrawAnimationPrefab);
            Card_Loader _Loader = _card.GetComponentInChildren<Card_Loader>();
            _Loader.addComponentAutomatically = false;
            _Loader.Set(card);
            CardPathAnim anim = new CardPathAnim(card, Discard_SFX, _card, GodDialogueTrigger.Discard);
            AnimationEventManager.getInstance.requestAnimation("Hand-Discard", _card, 0, anim);

            if(!exhausted.Contains(card))
                pDiscard.Add(card);
            
            pHand.Remove(card);

            anim.OnAnimCompletionTrigger.AddListener(OnDiscardChange.Invoke);
            return anim;
        }
        return null;
    }

    /// <summary>Completely randomizes the order of the library</summary>
    public void shuffleLibrary()
    {
        // prep for randomization
        List<CardPlayData> libraryCopy = new List<CardPlayData>();
        for (int i = 0; i < pLibrary.Count; i++)
        {
            libraryCopy.Add(pLibrary[i]);
        }
        pLibrary.Clear();

        // iterate through library copy and add them into the player library at random 
        for (int i = 0; i < libraryCopy.Count; i++)
        {
            int rnd = Random.Range(0, libraryCopy.Count);
            while(libraryCopy[rnd].CardType == null)
            {
                rnd = Random.Range(0, libraryCopy.Count);
            }
            pLibrary.Add(libraryCopy[rnd]);
            
            libraryCopy[rnd].Clear();
            // libraryCopy[rnd].CardType = null;
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
            cards[i] = Instantiate(CardShuffleAnimationPrefab);
            Card_Loader _Loader = cards[i].GetComponentInChildren<Card_Loader>();
            _Loader.addComponentAutomatically = false;
            _Loader.Set(pDiscard[i]);
            animations[i] = new CardPathAnim(_Loader._card, Shuffle_SFX, cards[i], GodDialogueTrigger.Shuffle);
            animations[i].OnAnimCompletionTrigger.AddListener(OnDiscardChange.Invoke);
            animations[i].OnAnimCompletionTrigger.AddListener(OnLibraryChange.Invoke);
        }
        // Request discard to library animations
        AnimationEventManager.getInstance.requestAnimation("ShuffleDiscard", cards, delay, animations);
        pDiscard.Clear();
        shuffleLibrary();
        OnShuffleDiscard?.Invoke();
        return animations;
    }

    public void SetCurrentDeck(DeckList_SO deckList_)
    {
        deckList = deckList_;
    }
}