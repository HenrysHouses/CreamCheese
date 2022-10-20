using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaviour : NonGod_Behaviour
{
    List<IMonster> targets = new List<IMonster>();
    Attack_Card currentCard;

    public override void Initialize(Card_SO card)
    {
        card_NonGod = (card as NonGod_Card);
        currentCard = card as Attack_Card;
        strengh = currentCard.baseStrength;
        this.card_abs = card;

        SendMessageUpwards("setBorder", Card_ClickGlowing.CardType.Attack);
    }

    public override void OnAction()
    {
        foreach (IMonster target in targets)
        {
            if (target != null)
            {
                target.DealDamage(strengh);
                //Debug.Log("Dealt " + strengh + " damage to " + target);
            }
        }
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (IMonster enemy in board.enemies)
        {
            enemy.IsObjectiveTo(this);
        }

        return base.Play(board);
    }

    protected override bool ReadyToBePlaced()
    {
        return targets.Count == 1;
    }

    public void AddTarget(IMonster enemy)
    {
        targets.Add(enemy);
    }
}
