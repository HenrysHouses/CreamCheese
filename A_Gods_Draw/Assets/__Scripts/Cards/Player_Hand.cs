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

    public GameObject CardHandPrefab;
    public class CardHandAnim
    {
        public Card_Selector Selector;
        public Animator cardAnimation;
        public Card_SO cardSO;
        public Card_Loader loader;

        public  CardHandAnim(Card_Selector selector, Card_Loader ldr)
        {
            this.cardAnimation = selector.GetComponentInChildren<Animator>();
            this.Selector = selector;
            this.cardSO = ldr.GetCardSO;
            loader = ldr;
        }
    }

    public void AddCard(Card_SO card)
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
    
    /// <summary></summary>
    /// <param name=""></param>
    /// <returns>Removed card's scriptable object</returns>
    public void RemoveCard(int index)
    {
        if (index >= CardSelectionAnimators.Count)
            return;

        CardSelectionAnimators[index].cardAnimation.enabled = false;
        CardSelectionAnimators.RemoveAt(index);
        UpdateCards();
    }
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
    private void Start()
    {
        CardHandPrefab.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
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
    public void UpdateCards()
    {
        float count = (float)CardSelectionAnimators.Count;
        for (int i = 0; i < CardSelectionAnimators.Count; i++)
        {
            var firstCardPos = ((CardSelectionAnimators.Count) * -0.05f);
            CardSelectionAnimators[i].Selector.transform.parent.localPosition = new Vector3(firstCardPos + (0.1f * i), 0, i * 0.005f);
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
        float rot = (float)cardRotation, count = (float)CardSelectionAnimators.Count;

        CardSelectionAnimators[index].Selector.transform.rotation = Quaternion.Euler(0, 0, (rot * ((count - 1) / 2f)) - rot * index);

        CardSelectionAnimators[index].cardAnimation.SetBool("ShowCard", false);
    }
}