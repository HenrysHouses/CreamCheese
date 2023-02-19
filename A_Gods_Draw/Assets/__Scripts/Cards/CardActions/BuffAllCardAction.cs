using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAllCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = true;
        yield break;
    }

    public override void CardPlaced(BoardStateController _board, ActionCard_Behaviour _source)
    {

        foreach(ActionCard_Behaviour _card in _board.placedCards)
        {

            if(_card.GetCardType == CardType.Attack)
                _card.Buff(_source.stats.strength, false);

        }
        
    }

}
