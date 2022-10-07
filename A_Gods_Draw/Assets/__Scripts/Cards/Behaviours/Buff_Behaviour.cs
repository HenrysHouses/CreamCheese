using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Behaviour : NonGod_Behaviour
{
    Buff_Card currentCard;

    int posInLane;

    public override void Initialize(Card_SO card)
    {
        card_NonGod = (card as NonGod_Card);
        currentCard = card as Buff_Card;
        strengh = currentCard.baseStrengh;
        this.card_abs = card;

        SendMessageUpwards("setBorder", Card_ClickGlowing.CardType.Buff);
    }

    public override void OnAction()
    {
    }

    public override IEnumerator OnPlay(BoardState board)
    {
        posInLane = manager.CurrentLane().Count;

        return base.OnPlay(board);
    }

    internal override void PlacedNextToThis(NonGod_Behaviour card)
    {
        card.GetBuff(currentCard.multiplyOGValue, strengh);
    }

    protected override bool ReadyToBePlaced()
    {
        return true;
    }

}

