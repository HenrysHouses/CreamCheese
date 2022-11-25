using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGodAction : Action
{
    public AttackGodAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.AttackGod;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        if (BoardStateController.playedGodCard != null)
            BoardStateController.playedGodCard.DealDamage(strengh, source);
    }
}
