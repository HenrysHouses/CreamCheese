//modified by Charlie

using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum CardType
{
    Attack,
    Defence,
    Buff,
    God
}

public class ChoosingReward : MonoBehaviour
{
    
    CardType currtype;

    DeckManager_SO deckManager;
    Card_Behaviour behaviour;

    public Transform spot1, spot2, spot3;

    private void Start()
    {
        behaviour = GetComponentInChildren<Card_Behaviour>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void GettingType(Map_Nodes mapNode)
    {
        switch (mapNode.Node.nodeType)
        {
            case NodeType.AttackReward:
                AttackRewardCards();
                break;
            case NodeType.DefenceReward:
                DefenceRewardCards();
                break;
            case NodeType.BuffReward:
                BuffRewardCards();
                break;
        }
    }

    private void OnMouseDown() //adds the card clicked on to the players deck
    {
        
    }

    void AttackRewardCards()
    {
        if (currtype.Equals(CardType.Attack))
        {
            //FullCardList.CardSearch(new NonGod_Card(), CardType.Attack.ToString(), null);
        }
    }

    void DefenceRewardCards()
    {
        if (currtype.Equals(CardType.Defence))
        {
            //FullCardList.CardSearch(new NonGod_Card(), CardType.Defence.ToString(), null);
        }
    }

    void BuffRewardCards()
    {
        if (currtype.Equals(CardType.Buff))
        {
            //FullCardList.CardSearch(new NonGod_Card(), CardType.Buff.ToString(), null);
        }
    }

    /*todo switch state for the different types
     * */

}
