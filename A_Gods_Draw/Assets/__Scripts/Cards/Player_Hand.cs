using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : MonoBehaviour
{
    public TurnManager _turnManager;
    private float cardRotation = 10; 
    public List<CardInHand> CAH = new List<CardInHand>();
    public List<Card_Behaviour> behaviours = new();

    public GameObject CardinHandPrefab;
    public class CardInHand
    {
        public Card_Selector CS;
        public Animator cardAnimation;

        public  CardInHand(Card_Selector selector)
        {
            this.cardAnimation = selector.GetComponentInChildren<Animator>();
            this.CS = selector;
        }
    }

    public void AddCard(Card_SO card)
    {  
        GameObject spawn = Instantiate(CardinHandPrefab);
        Card_Loader _loader = spawn.GetComponentInChildren<Card_Loader>();
        _loader.Set(card, _turnManager);
        CardInHand _card = new CardInHand(spawn.GetComponentInChildren<Card_Selector>());
        CAH.Add(_card);  
        
        UpdateCards();

        Debug.Log("Card Added to hand");
    }
    

    public void RemoveCard(int pos)
    {
         
        CAH.RemoveAt(pos);
        UpdateCards();     
        
    }

    private void Start()
    {   


    }


    private void Update()
    {
        for (int i = 0; i < CAH.Count; i++)
        {
            if(CAH[i].CS.holdingOver)   
            {
                HoverOverCard(i);
            }
            else
            {
                StopHover(i);
            }
        }
    }
    private void UpdateCards()
    {
        for (int i = 0; i < CAH.Count; i++)
        {
            CAH[i].CS.transform.rotation = Quaternion.Euler(cardRotation * i,0,0);
        }
    }
    
    void HoverOverCard(int card)
    {
        Debug.Log("HoveringOver");
        CAH[card].CS.transform.rotation = Quaternion.Euler(0,0,0);
        CAH[card].cardAnimation.SetBool("ShowCard",true);
    }
    void StopHover(int card)
    {
        
        CAH[card].CS.transform.rotation = Quaternion.Euler(cardRotation * card, 0, 0);
        CAH[card].cardAnimation.SetBool("ShowCard", false);
    }
}
