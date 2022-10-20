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

    List <IMonster> targets = new List<IMonster>();

    public override void Initialize(Card_SO card)
    {
        card_NonGod = (card as NonGod_Card);
        currentCard = card as Defense_Card;
        strengh = currentCard.baseStrength;
        this.card_abs = card;

        SendMessageUpwards("setBorder", Card_ClickGlowing.CardType.Defence);
    }

    public override void OnAction()
    {
     //   foreach (IMonster target in targets)            defends the specific action here?
     //   {
     //       if(target != null)
     //       {
     //           target.DealDamage()
     //       }
     //   }
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        if (board.playedGodCard)
        {
            board.playedGodCard.CanBeDefendedBy(this);
        }
        board.player.CanBeDefended(this);

        yield return base.Play(board);
    }

    protected override bool ReadyToBePlaced()
    {
        return this.player || this.god_card;
    }

    internal void ItDefends(PlayerController playerController = null, God_Behaviour god = null)
    {
        if (playerController)
        {
            player = playerController;
        }
        else if (god)
        {
            god_card = god;
        }
    }

    public override void LatePlayed(BoardStateController board)
    {
        base.LatePlayed(board);

        if (player)
        {
            player.Defend(strengh);
        }
        else if (god_card)
        {
            god_card.Defend(strengh);
        }
    }
}

