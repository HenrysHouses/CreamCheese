/*
 * Edited by
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : MonoBehaviour
{
    [SerializeField]
    Transform handPlace;

    // public TurnManager _turnManager; // ! this will be phased out after refactoring

    private float cardRotation = 10; 
    public List<CardHandAnim> CardSelectionAnimators = new List<CardHandAnim>();
    public List<Card_Loader> cardLoaders = new List<Card_Loader>();

    public GameObject CardHandPrefab;
    public class CardHandAnim
    {
        public Card_Selector Selector;
        public Animator cardAnimation;
        public Card_SO cardSO;

        public  CardHandAnim(Card_Selector selector, Card_SO SO)
        {
            this.cardAnimation = selector.GetComponentInChildren<Animator>();
            this.Selector = selector;
            this.cardSO = SO;
        }
    }

    public void AddCard(Card_SO card)
    {
        float posX = handPlace.position.x;
        handPlace.position += Vector3.right * (-0.3f + CardSelectionAnimators.Count * 0.1f);
        float posZ = handPlace.position.z;
        handPlace.position += Vector3.forward * (0.0001f + CardSelectionAnimators.Count * 0.01f / 2.5f); // << This puts the cards behing eacother, but makes unity angery
        GameObject spawn = Instantiate(CardHandPrefab, handPlace.position, Quaternion.identity);
        CardHandPrefab.transform.localScale = new Vector3(0.75f,0.75f,0.75f);
        Card_Loader _loader = spawn.GetComponentInChildren<Card_Loader>();
        _loader.Set(card);
        CardHandAnim _card = new CardHandAnim(spawn.GetComponentInChildren<Card_Selector>(), _loader.GetCardSO);
        handPlace.position = new Vector3(posX, handPlace.position.y, posZ);

        spawn.transform.parent = handPlace;

        CardSelectionAnimators.Add(_card);

        spawn.transform.GetComponentInChildren<BoxCollider>().enabled = true;

        cardLoaders.Add(spawn.GetComponentInChildren<Card_Loader>());

        // Debug.Log("Card in hand: " + spawn.name + ", this one is number: " + (CAH.Count - 1));
        
        UpdateCards();

        //Debug.Log("Card Added to hand");
    }
    
    /// <summary></summary>
    /// <param name=""></param>
    /// <returns>Removed card's scriptable object</returns>
    public void RemoveCard(int index)
    {
        if (index >= CardSelectionAnimators.Count || index >= cardLoaders.Count)
            return;

        CardSelectionAnimators[index].cardAnimation.enabled = false;
        Destroy(CardSelectionAnimators[index].Selector.gameObject);
        CardSelectionAnimators.RemoveAt(index);
        cardLoaders.RemoveAt(index);
        UpdateCards();
    }

    public void RemoveAllCards()
    {
        CardSelectionAnimators.Clear();
        UpdateCards();
    }
    private void Start()
    {   


    }


    private void Update()
    {
        for (int i = 0; i < CardSelectionAnimators.Count; i++)
        {
            if(CardSelectionAnimators[i].Selector.holdingOver)   
            {
                HoverOverCard(i);
            }
            else
            {
                StopHover(i);
            }
        }
    }
    public void UpdateCards()
    {
        float count = (float)CardSelectionAnimators.Count;
        for (int i = 0; i < CardSelectionAnimators.Count; i++)
        {
            CardSelectionAnimators[i].Selector.transform.rotation = Quaternion.Euler(0, 0, (cardRotation * ((count - 1) / 2f)) - cardRotation * i);
            
        }
        
    }
    
    void HoverOverCard(int index)
    {
        //Debug.Log("HoveringOver");
        CardSelectionAnimators[index].Selector.transform.rotation = Quaternion.Euler(0,0,0);
        CardSelectionAnimators[index].cardAnimation.SetBool("ShowCard",true);
    }
    void StopHover(int index)
    {
        Card_Behaviour CAB = CardSelectionAnimators[index].Selector.GetComponentInChildren<Card_Behaviour>();
        if(!CAB)
        {
            float rot = (float)cardRotation, count = (float)CardSelectionAnimators.Count;
        CardSelectionAnimators[index].Selector.transform.rotation = Quaternion.Euler(0, 0, (rot * ((count - 1) / 2f)) - rot * index);
        CardSelectionAnimators[index].cardAnimation.SetBool("ShowCard", false);
        }
    }
}