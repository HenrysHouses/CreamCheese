using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using FMODUnity;

public class DeckController : MonoBehaviour
{
    // Animation and SFX
    [SerializeField, Tooltip("Prefab used in animations for cards")]
    GameObject CardDrawAnimationPrefab;
    [SerializeField, Tooltip("Prefab used in animations for cards")]
    GameObject CardShuffleAnimationPrefab;
    [SerializeField] EventReference Draw_SFX;
    [SerializeField] EventReference Discard_SFX;
    [SerializeField] EventReference Shuffle_SFX;

    [HideInInspector] public UnityEvent OnLibraryChange;
    [HideInInspector] public UnityEvent OnDiscardChange;
    [HideInInspector] public UnityEvent OnShuffleDiscard;

    private void Awake()
    {
        OnLibraryChange = new UnityEvent();
        OnDiscardChange = new UnityEvent();
    }

#region ------ Card References ------

    [field:SerializeField, Tooltip("Cards the player has obtained")]
    public DeckList_SO deckList;
    public DeckListData deckData => deckList.deckData;
    static DeckList_SO starterDeck;

    public static DeckList_SO getStarterDeck()
    {
        if(starterDeck != null)
        {
            return starterDeck;
        }

        starterDeck = Resources.Load<DeckList_SO>("DeckLists/StarterDeck_Tyr");
        Debug.Log("Loaded starter deck!");
        return starterDeck;
    }

    // Load the deck list from Assets/Resources/DeckLists
    void OnValidate()
    {
        if (deckList == null)
        {
            deckList = Resources.Load<DeckList_SO>("DeckLists/DeckList");
        }
    }
#endregion
#region ------ Card Handling at runtime ------

    [SerializeField, Tooltip("Cards the player can draw")] 
    List<CardPlayData> pLibrary;
    
    public float libraryCount => pLibrary.Count;
    [SerializeField, Tooltip("Cards the player has discarded")] 
    List<CardPlayData> pDiscard;
    
    public float discardCount => pDiscard.Count;
    [SerializeField, Tooltip("Cards in the player's current hand")] 
    List<CardPlayData> pHand;
    public float handCount => pHand.Count;
    [SerializeField]List<CardPlayData> pBoard;
    public float cardsCount => handCount + discardCount + libraryCount + pBoard.Count;

    public CardPlayData[] GetHand()
    {
        return pHand.ToArray();
    }

    /// <summary>Clear all card lists the player uses for combat</summary>
    public void clear()
    {
        pLibrary.Clear();
        pDiscard.Clear();
        pHand.Clear();
        pBoard.Clear();
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
        pBoard.Clear();
    }

    ///<summary>Adds a card to the Library for this encounter</summary>
    public void AddCardToLib(CardPlayData card)
    {
        pLibrary.Add(card);
    }

    public void setCurrentDeck(DeckList_SO playerDeck)
    {
        deckList = playerDeck.Clone();
    }

    public void MoveCardToBoard(CardPlayData card)
    {
        pHand.Remove(card);
        pBoard.Add(card);
    }

    public void TransferExperienceToHand(CardPlayData UniqueCard)
    {
        for (int i = 0; i < pHand.Count; i++)
        {
            if(pHand[i].Experience.ID == UniqueCard.Experience.ID)
            {
                pHand[i] = new CardPlayData(UniqueCard);
                return;
            }
        }
    }

    #region ------ Card Draw/Discard/Shuffle ------
    
    /// <summary>Move the top card/s of the player library to the player hand. Trigger card draw animations</summary>
    /// <param name="amount">The amount of cards to draw</param>
    /// <returns>List of all animation OnCompletionEvents</returns>
    public CardPathAnim[] drawCard(int amount, float delay)
    {
        if (pLibrary.Count <= 0) // if there is not enough cards in library to draw, shuffle the discard into the library and return
        {
            return null;
        }

        if(pLibrary.Count < amount)
        {
            amount = pLibrary.Count;
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
            // Debug.LogError(pLibrary[0].Experience.ID);
            animations[i] = new CardPathAnim(pLibrary[0], Draw_SFX, cards[i], GodDialogueTrigger.Draw);
            pLibrary.Remove(pLibrary[0]);
        }
        OnLibraryChange?.Invoke();
        AnimationEventManager.getInstance.requestAnimation("Library-Hand", cards, delay, animations);
        return animations;
    }

    /// <summary>Moves a card currently in player hand to player discard. Trigger discard animation</summary>
    public CardPathAnim[] discardCard(CardPlayData[] cardData, float delay)
    {
        CardPathAnim[] animations = new CardPathAnim[cardData.Length];

        for (int i = 0; i < cardData.Length; i++)
        {
            if (pHand.Contains(cardData[i]))
            {
                // preps the discard animation
                GameObject _card = Instantiate(CardDrawAnimationPrefab);
                Card_Loader _Loader = _card.GetComponentInChildren<Card_Loader>();
                _Loader.addComponentAutomatically = false;
                _Loader.Set(cardData[i]);
                animations[i] = new CardPathAnim(cardData[i], Discard_SFX, _card, GodDialogueTrigger.Discard);
                AnimationEventManager.getInstance.requestAnimation("Hand-Discard", _card, delay*i, animations[i]);
                
                pDiscard.Add(cardData[i]);
                pHand.Remove(cardData[i]);

                animations[i].OnAnimCompletionTrigger.AddListener(OnDiscardChange.Invoke);
            }
        }

        if(animations.Length == 0)
            return null;

        return animations;
    }

    public CardPathAnim DiscardCardOnBoard(CardPlayData cardData, float delay, bool exhausted)
    {
        CardPathAnim animation = null;
        int index = -1;

        for (int i = 0; i < pBoard.Count; i++)
        {
            if(pBoard[i].Experience.ID == cardData.Experience.ID)
                index = i;
        }

        if (index != -1)
        {
            // preps the discard animation
            GameObject _card = Instantiate(CardDrawAnimationPrefab);
            Card_Loader _Loader = _card.GetComponentInChildren<Card_Loader>();
            _Loader.addComponentAutomatically = false;
            _Loader.Set(cardData);
            animation = new CardPathAnim(cardData, Discard_SFX, _card, GodDialogueTrigger.Discard);
            AnimationEventManager.getInstance.requestAnimation("Hand-Discard", _card, delay, animation);
            
            pBoard.RemoveAt(index);

            if(!exhausted)
                pDiscard.Add(cardData); // dont add exhausted cards here
            else
                Debug.Log("Exhausted: " + cardData.CardType.cardName);
            animation.OnAnimCompletionTrigger.AddListener(OnDiscardChange.Invoke);
        }
        return animation;
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
            
            libraryCopy[rnd] = new CardPlayData(null);
        }
    }

    /// <summary>Moves all cards from discard to the library, Then shuffle the library</summary>
    /// <returns>List of all animation OnCompletionEvents</returns>
    public CardPathAnim[] shuffleDiscard(float delay)
    {
        // move discard to library, prep for animations
        GameObject[] cards = new GameObject[pDiscard.Count];
        CardPathAnim[] animations = new CardPathAnim[pDiscard.Count];
        
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

        if(animations.Length == 0)
            return null;

        return animations;
    }
    
    #endregion
#endregion
}
