using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense_Behaviour : NonGod_Behaviour
{
    //Damageable a;
    Defense_Card currentCard;

    PlayerController player;
    God_Behaviour god_card;

    public override void Initialize(Card_SO card)
    {
        current = (card as NonGod_Card);
        currentCard = card as Defense_Card;
        strengh = currentCard.baseStrengh;
        this.card = card;

        SendMessageUpwards("setBorder", Card_ClickGlowing.CardType.Defence);
    }

    public override void OnAction()
    {
        if (player)
        {
            player.Defend(strengh);
            Debug.Log("Defended " + player + " for " + strengh);
        }
    }

    public override IEnumerator OnPlay(BoardState board)
    {
        if (board.currentGod)
        {
            board.currentGod.CanBeDefendedBy(this);
        }
        board.player.CanBeDefended(this);

        yield return base.OnPlay(board);
    }

    protected override bool ReadyToBePlaced()
    {
        return this.player || this.god_card;
    }

    internal int ItDefends(PlayerController playerController = null, God_Behaviour god = null)
    {
        if (playerController)
        {
            player = playerController;
        }
        else if (god)
        {
            god_card = god;
        }
        return strengh;
    }
}

