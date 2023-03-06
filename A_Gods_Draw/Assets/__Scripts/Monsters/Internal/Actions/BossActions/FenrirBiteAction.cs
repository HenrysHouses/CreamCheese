/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirBiteAction : MonsterAction
{

    public FenrirBiteAction (int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.FenrirBite;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Shield_IMG_v1");
        desc = "This enemy will rip apart one of your played attack or defence cards";

    }

    public override void Execute(BoardStateController _board, int _strengh, UnityEngine.Object _source)
    {

        List<ActionCard_Behaviour> _cards = _board.placedCards;

        int _cardIndex = Random.Range(0, _cards.Count);

        _cards[_cardIndex].RemoveFromHand(); 

    }

}
*/