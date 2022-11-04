using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGodAction : Action
{
    public AttackGodAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.AttackGod;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/15");
    }

    public override void Execute(BoardStateController BoardStateController, int strengh)
    {
        if (BoardStateController.playedGodCard != null)
            BoardStateController.playedGodCard.DealDamage(strengh);
    }
}
