// Written by Javier

using UnityEngine;

public class BuffAttackersAction : Action
{
    public BuffAttackersAction()
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will buff enemies that want to attack";
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
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