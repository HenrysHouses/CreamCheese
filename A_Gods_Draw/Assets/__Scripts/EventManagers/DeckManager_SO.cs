/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;



[CreateAssetMenu(menuName = "Events/DeckManager")]
public class DeckManager_SO : ScriptableObject
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] AnimationManager_SO animationManager;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] DeckList_SO deckList;
    [SerializeField] List<Card_SO> pLibrary;
    [SerializeField] List<Card_SO> pDiscard;
    [SerializeField] List<Card_SO> pHand;


    [System.NonSerialized]
    public UnityEvent deckListChangeEvent; 
    [System.NonSerialized]
    public UnityEvent pLibraryChangeEvent;
    [System.NonSerialized]
    public UnityEvent pDiscardChangeEvent;
    [System.NonSerialized]
    public UnityEvent pHandChangeEvent;

    public void SetTurnManager(TurnManager manager)
    {
        turnManager = manager;
    }

    void OnValidate()
    {
        if (deckList == null)
            deckList = Resources.Load<DeckList_SO>("DeckLists/DeckList");
    }

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

    void OnDisable()
    {
        clear();
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
        pLibrary.Clear();
        for (int i = 0; i < deckList.Deck.Count; i++)
        {
            pLibrary.Add(deckList.Deck[i]);
        }
        pHand.Clear();
        pDiscard.Clear();
    }

    public void clear()
    {
        pLibrary.Clear();
        pDiscard.Clear();
        pHand.Clear();
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
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            pHand.Add(pLibrary[0]);
            GameObject card = Instantiate(CardPrefab);
            card.GetComponentInChildren<Transform>().GetComponentInChildren<Card_Loader>().Set(pLibrary[0], turnManager);
            pLibrary.Remove(pLibrary[0]);

            PathAnimatorController.pathAnimation animation = new PathAnimatorController.pathAnimation();
            animation.CompletionTrigger.AddListener(AnimationComplete);

            animationManager.requestAnimation("Library-Hand", card);

            //Just to make them clickable
            card.transform.position = new Vector3(-0.2f + i * 0.1f, 0.1f, -0.3f);
            //card.transform.rotation = Quaternion.Euler(-20 + i * 10, 90, 0);
        }
        pLibraryChangeEvent.Invoke();
        pHandChangeEvent.Invoke();
    }

    public void discardAll()
    {
        GameObject[] cards = new GameObject[pHand.Count];
        for (int i = 0; i < pHand.Count; i++)
        {
            pDiscard.Add(pHand[i]);
            cards[i] = Instantiate(CardPrefab);
            cards[i].GetComponentInChildren<Card_Loader>().Set(pHand[i], turnManager);
        }
        animationManager.requestAnimation("Hand-Discard", cards, 0, 0.25f);
        pHand.Clear();
        pHandChangeEvent.Invoke();
        pDiscardChangeEvent.Invoke();
    }

    public void discardCard(Card_SO card)
    {
        if(pHand.Contains(card))
        {
            GameObject _card = Instantiate(CardPrefab);
            _card.GetComponentInChildren<Card_Loader>().Set(card, turnManager);
            animationManager.requestAnimation("Hand-Discard", _card);
            
            pHand.Remove(card);
            pDiscard.Add(card);
        }
        pHandChangeEvent.Invoke();
        pDiscardChangeEvent.Invoke();
    }

    public void shuffleLibrary()
    {
        List<Card_SO> libraryCopy = new List<Card_SO>();
        for (int i = 0; i < pLibrary.Count; i++)
        {
            libraryCopy.Add(pLibrary[i]);
        }

        pLibrary.Clear();
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
        GameObject[] cards = new GameObject[pDiscard.Count];
        for (int i = 0; i < pDiscard.Count; i++)
        {
            pLibrary.Add(pDiscard[i]);
            cards[i] = Instantiate(CardPrefab);
            cards[i].GetComponentInChildren<Card_Loader>().Set(pDiscard[i], turnManager);
        }
        animationManager.requestAnimation("ShuffleDiscard", cards, 0, 0.18f);
        pDiscard.Clear();
        pDiscardChangeEvent.Invoke();
        shuffleLibrary();
    }

    public List<Card_SO> GetCurrentHand()
    {
        return pHand;
    }

    public void AnimationComplete() { }

}