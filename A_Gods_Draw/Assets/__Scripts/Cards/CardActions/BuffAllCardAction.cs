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
        playSFX(_source.gameObject);

        foreach(ActionCard_Behaviour _card in _board.placedCards)
        {

            if(_card.GetCardType == CardType.Attack)
            {
                _card.Buff(_source.stats.strength, false);
                _card.UpdateQueuedDamage(true);
            }
        }
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
        Debug.LogError("Buff All has no VFX");
    }
}
