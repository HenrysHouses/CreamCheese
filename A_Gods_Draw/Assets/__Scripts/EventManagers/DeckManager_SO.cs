using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;



[CreateAssetMenu(menuName = "Events/DeckManager")]
public class DeckManager_SO : ScriptableObject
{
    [SerializeField]
    AnimationManager_SO animationManager;
    [SerializeField]
    DeckList_SO deckList;
    [SerializeField]
    List<Card_SO> pLibrary;
    [SerializeField]
    List<Card_SO> pDiscard;
    [SerializeField]
    List<Card_SO> pHand;


    [System.NonSerialized]
    public UnityEvent deckListChangeEvent; 
    [System.NonSerialized]
    public UnityEvent pLibraryChangeEvent;
    [System.NonSerialized]
    public UnityEvent pDiscardChangeEvent;
    [System.NonSerialized]
    public UnityEvent pHandChangeEvent;
    [System.NonSerialized]
    public UnityEvent DrawEvent;
    [System.NonSerialized]
    public UnityEvent DiscardEvent;
    [System.NonSerialized]
    public UnityEvent RecycleEvent;

    void OnValidate()
    {
        if (deckList == null)
            deckList = Resources.Load<DeckList_SO>("DeckLists/DeckList");
    }



    void OnEnable()
    {
        if(deckListChangeEvent == null)
            deckListChangeEvent = new UnityEvent();

        pLibrary = new List<Card_SO>();
        if(pLibraryChangeEvent == null)
            pLibraryChangeEvent = new UnityEvent();
        
        pDiscard = new List<Card_SO>();
        if(pDiscardChangeEvent == null)
            pDiscardChangeEvent = new UnityEvent();
        
        pHand = new List<Card_SO>();
        if(pHandChangeEvent == null)
            pHandChangeEvent = new UnityEvent();

        if(DrawEvent == null)
            DrawEvent = new UnityEvent();
        // DrawEvent += 

        if(DiscardEvent == null)
            DiscardEvent = new UnityEvent();

        if(RecycleEvent == null)
            RecycleEvent = new UnityEvent();
    }

    public void addCardToDeck(Card_SO card)
    {
        deckList.Deck.Add(card);
        deckListChangeEvent.Invoke();
    }

    public void removeCardFromDeck(Card_SO card)
    {
        deckList.Deck.Remove(card);
        deckListChangeEvent.Invoke();
    }

    public void reset()
    {
        pLibrary = new List<Card_SO>();
        for (int i = 0; i < deckList.Deck.Count; i++)
        {
            pLibrary.Add(deckList.Deck[i]);
        }
        pHand = new List<Card_SO>();
        pDiscard = new List<Card_SO>();
    }

    public void clear()
    {
        pLibrary = new List<Card_SO>();
        pDiscard = new List<Card_SO>();
        pHand = new List<Card_SO>();
    }


    public void addCardToLibrary(Card_SO card)
    {
        pLibrary.Add(card);
        pLibraryChangeEvent.Invoke();
    }

    public void removeCardFromLibrary(Card_SO card)
    {
        pLibrary.Remove(card);
        pLibraryChangeEvent.Invoke();
    }

    public void addCardToDiscard(Card_SO card)
    {
        pDiscard.Add(card);
        pDiscardChangeEvent.Invoke();
    }

    public void removeCardFromDiscard(Card_SO card)
    {
        pDiscard.Remove(card);
        pDiscardChangeEvent.Invoke();
    }
    
    public void addCardToHand(Card_SO card)
    {
        pHand.Add(card);
        pHandChangeEvent.Invoke();
    }

    public void removeCardFromHand(Card_SO card)
    {
        pHand.Remove(card);
        pHandChangeEvent.Invoke();
    }

    public void drawCard(int amount)
    {
        if (pLibrary.Count < amount)
        {
            shuffleDiscard();
        }
        for (int i = 0; i < amount; i++)
        {
            pHand.Add(pLibrary[0]);
            pLibrary.Remove(pLibrary[0]);
            GameObject card = new GameObject("TestDrawer");
            animationManager.requestAnimation("Library-Hand", card);
        }
        pLibraryChangeEvent.Invoke();
        pHandChangeEvent.Invoke();
    }

    public void discardAll()
    {
        for (int i = 0; i < pHand.Count; i++)
        {
            pDiscard.Add(pHand[i]);
            DiscardEvent.Invoke();
        }
        pHand = new List<Card_SO>();
        pHandChangeEvent.Invoke();
        pDiscardChangeEvent.Invoke();
    }

    public void discardCard(Card_SO card)
    {
        if(pHand.Contains(card))
        {
            pHand.Remove(card);
            pDiscard.Add(card);
        }
        pHandChangeEvent.Invoke();
        pDiscardChangeEvent.Invoke();
        DiscardEvent.Invoke();
    }

    public void shuffleLibrary()
    {
        List<Card_SO> libraryCopy = new List<Card_SO>();
        for (int i = 0; i < pLibrary.Count; i++)
        {
            libraryCopy.Add(pLibrary[i]);
        }
        pLibrary = new List<Card_SO>();
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
        // pLibrary = randomized;
        pLibraryChangeEvent.Invoke();
    }
    
    public void shuffleDiscard()
    {
        foreach (var card in pDiscard)
        {
            pLibrary.Add(card);
        }
        pDiscard.Clear();
        pDiscardChangeEvent.Invoke();
        shuffleLibrary();
    }

    public List<Card_SO> GetCurrentHand()
    {
        return pHand;
    }
}