/*
 * Refactored by
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : MonoBehaviour
{
    [SerializeField]
    Transform handPlace;
    private float cardRotation = 5; 
    public List<CardHandAnim> CardSelectionAnimators = new List<CardHandAnim>();
    public GameObject CardHandPrefab;

    /// <summary>Data container for on hover animations</summary>
    public class CardHandAnim
    {
        public Card_Selector Selector;
        public Animator cardAnimation;
        public CardPlayData _card;
        public Card_Loader loader;

        public  CardHandAnim(Card_Selector selector, Card_Loader loader)
        {
            this.cardAnimation = selector.GetComponentInChildren<Animator>();
            this.Selector = selector;
            this._card = loader._card;
            this.loader = loader;
        }
    }

    /// <summary>Instantiates a card into the player's hand</summary>
    /// <param name="card">ScriptableObject for the card</param>
    public void AddCard(CardPlayData card)
    {
        GameObject spawn = Instantiate(CardHandPrefab, handPlace.position, Quaternion.identity);
        spawn.transform.SetParent(handPlace);

        Card_Loader _loader = spawn.GetComponentInChildren<Card_Loader>();
        _loader.Set(card);

        CardHandAnim _card = new CardHandAnim(spawn.GetComponentInChildren<Card_Selector>(), _loader);

        CardSelectionAnimators.Add(_card);

        spawn.transform.GetComponentInChildren<BoxCollider>().enabled = true;
        
        UpdateCards();
    }
    
    /// <summary>Removes a card from the player's hand at index</summary>
    /// <param name="index">Index of card to remove</param>
    public void RemoveCard(int index)
    {
        if (index >= CardSelectionAnimators.Count)
            return;

        CardSelectionAnimators[index].cardAnimation.enabled = false;
        CardSelectionAnimators.RemoveAt(index);
        UpdateCards();
    }

    /// <summary>Removes a card from the player's hand</summary>
    /// <param name="loader">The target card's Card_Loader that should be removed</param>
    public void RemoveCard(Card_Loader loader)
    {
        int index = 0;
        while (index < CardSelectionAnimators.Count)
        {
            if (CardSelectionAnimators[index].loader == loader)
            {
                break;
            }
            index++;
        }

        if (index >= CardSelectionAnimators.Count)
            return;

        CardSelectionAnimators[index].cardAnimation.enabled = false;
        CardSelectionAnimators.RemoveAt(index);
        UpdateCards();
    }

    public void RemoveAllCards()
    {
        CardSelectionAnimators.Clear();
        UpdateCards();
    }

    private void Update()
    {
        for (int i = 0; i < CardSelectionAnimators.Count; i++)
        {
            if (CardSelectionAnimators[i].Selector.holdingOver)   
            {
                HoverOverCard(i);
            }
            else
            {
                StopHover(i);
            }
        }
    }

    /// <summary>Calculates and updates the rotations and positions of the cards in the player's hand</summary>
    public void UpdateCards()
    {
        float count = (float)CardSelectionAnimators.Count;
        int cardoffset = 1;
        if(CardSelectionAnimators.Count % 2 == 0)
        {
            cardoffset = 0;
        }
        for (int i = 0; i < CardSelectionAnimators.Count; i++)
        {
            var firstCardPos = ((CardSelectionAnimators.Count) * -0.05f);
            CardSelectionAnimators[i].Selector.setHandPos(new Vector3(firstCardPos + (0.15f * i), 0f,Mathf.PingPong(((float)(i) * 2)  / (CardSelectionAnimators.Count - cardoffset) ,1f) * -0.05f));
           // Debug.Log(Mathf.PingPong((i * 2)  / CardSelectionAnimators.Count,1) + "I is : " + i);
            // CardSelectionAnimators[i].Selector.transform.localPosition = (new Vector3(firstCardPos + (0.1f * i), 0, i * 0.005f));
            
            CardSelectionAnimators[i].Selector.transform.rotation = Quaternion.Euler(0, 0, (cardRotation * ((count - 1) / 2f)) - cardRotation * (float)i);
        }
    }
    
    /// <summary>Starts on hover animation on a card at index</summary>
    /// <param name="index">Index of a card in hand</param>
    void HoverOverCard(int index)
    {
        //Debug.Log("HoveringOver");
        CardSelectionAnimators[index].Selector.transform.rotation = Quaternion.Euler(0,0,0);
        CardSelectionAnimators[index].cardAnimation.SetBool("ShowCard",true);
    }

    /// <summary>Stops on hover animation on a card at index</summary>
    /// <param name="index">Index of a card in hand</param>
    void StopHover(int index)
    {
        float rot = (float)cardRotation, count = (float)CardSelectionAnimators.Count;

        CardSelectionAnimators[index].Selector.transform.rotation = Quaternion.Euler(0, 0, (rot * ((count - 1) / 2f)) - rot * index);

        CardSelectionAnimators[index].cardAnimation.SetBool("ShowCard", false);
    }
}