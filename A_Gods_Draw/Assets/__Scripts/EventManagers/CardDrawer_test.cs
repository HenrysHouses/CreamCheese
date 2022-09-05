using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardDrawer_test : MonoBehaviour
{
    [SerializeField]
    private DeckManager_SO deckManager;
    [SerializeField]
    private Card_SO selectedCard;

    public void addCardTest()
    {
        deckManager.addCardToDeck(selectedCard);
    }

    public void createLibrary()
    {
        deckManager.reset();
    }

    public void DrawACard(int amount)
    {
        deckManager.drawCard(amount);
    }

    public void DiscardACard()
    {
        deckManager.discardCard(selectedCard);
    }

    public void DiscardHand()
    {
        deckManager.discardAll();
    }

    public void Shuffle()
    {
        deckManager.shuffleLibrary();
    }

    public void Recycle()
    {
        deckManager.shuffleDiscard();
    }
    





    void OnEnable()
    {
        deckManager.deckListChangeEvent.AddListener(deckPrint);
    }

    void OnDisable()
    {
        deckManager.deckListChangeEvent.RemoveListener(deckPrint);
    }

    public void deckPrint()
    {
        Debug.Log(selectedCard + "changed in deck");
    }


}
