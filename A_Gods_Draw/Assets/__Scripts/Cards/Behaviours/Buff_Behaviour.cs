using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Behaviour : NonGod_Behaviour
{
    Buff_Card currentCard;

    int posInLane;

    public override void Initialize(Card_SO card)
    {
        current = (card as NonGod_Card);
        currentCard = card as Buff_Card;
        strengh = currentCard.baseStrengh;
        this.card = card;

        SendMessageUpwards("setBorder", Card_ClickGlowing.CardType.Buff);
    }

    public override void OnAction()
    {
        if (posInLane < manager.CurrentLane().Count - 1)
        {
            manager.CurrentLane()[posInLane + 1].GetBuff(currentCard.multiplyOGValue, strengh);
        }
    }

    public override IEnumerator OnPlay(BoardState board)
    {
        posInLane = manager.CurrentLane().Count;

        return base.OnPlay(board);
    }

    protected override bool ReadyToBePlaced()
    {
        return true;
    }

}

