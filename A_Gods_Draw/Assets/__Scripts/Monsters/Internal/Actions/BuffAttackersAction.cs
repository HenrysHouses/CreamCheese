using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAttackersAction : Action
{
    public BuffAttackersAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/32");
    }

    public override void Execute(BoardStateController BoardStateController, int strengh)
    {
        foreach (IMonster enemy in BoardStateController.getLivingEnemies())
        {
            if (enemy.GetIntent().GetID() == EnemyIntent.AttackPlayer || enemy.GetIntent().GetID() == EnemyIntent.AttackGod)
            {
                enemy.Buff(strengh);
            }
        }
    }
}