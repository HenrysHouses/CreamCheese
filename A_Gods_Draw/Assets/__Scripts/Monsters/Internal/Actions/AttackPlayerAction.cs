using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerAction : Action
{
    public AttackPlayerAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/39");
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        BoardStateController.Player.DealDamage(strengh);
    }
}
