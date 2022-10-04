using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : MonoBehaviour
{
    [SerializeField]
    Transform handPlace;

    public TurnManager _turnManager;
    private float cardRotation = 20; 
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
        float posX = handPlace.position.x;
        handPlace.position += Vector3.right * (-0.3f + CAH.Count * 0.15f);
        GameObject spawn = Instantiate(CardinHandPrefab, handPlace.position, Quaternion.identity);
        Card_Loader _loader = spawn.GetComponentInChildren<Card_Loader>();
        _loader.Set(card, _turnManager);
        CardInHand _card = new CardInHand(spawn.GetComponentInChildren<Card_Selector>());
        handPlace.position = new Vector3(posX, handPlace.position.y, handPlace.position.z);
        CAH.Add(_card);

        spawn.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;

        behaviours.Add(spawn.GetComponentInChildren<Card_Behaviour>());

        // Debug.Log("Card in hand: " + spawn.name + ", this one is number: " + (CAH.Count - 1));
        
        UpdateCards();

        //Debug.Log("Card Added to hand");
    }
    

    public void RemoveCard(int pos)
    {
        if (pos >= CAH.Count)
        {
            return;
        }
        CAH[pos].cardAnimation.enabled = false;
        CAH.RemoveAt(pos);
        UpdateCards();
    }

    public void RemoveAllCards()
    {
        CAH.Clear();
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
        float count = (float)CAH.Count;
        for (int i = 0; i < CAH.Count; i++)
        {
            CAH[i].CS.transform.rotation = Quaternion.Euler(0, 0, (cardRotation * ((count - 1) / 2f)) - cardRotation * i);
        }
    }
    
    void HoverOverCard(int card)
    {
        //Debug.Log("HoveringOver");
        CAH[card].CS.transform.rotation = Quaternion.Euler(0,0,0);
        CAH[card].cardAnimation.SetBool("ShowCard",true);
    }
    void StopHover(int card)
    {
        float rot = (float)cardRotation, count = (float)CAH.Count;
        CAH[card].CS.transform.rotation = Quaternion.Euler(0, 0, (rot * ((count - 1) / 2f)) - rot * card);
        CAH[card].cardAnimation.SetBool("ShowCard", false);
    }
}
