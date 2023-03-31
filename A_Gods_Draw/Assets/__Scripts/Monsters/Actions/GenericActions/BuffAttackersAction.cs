// Written by Javier

using UnityEngine;
using FMODUnity;

public class BuffAttackersAction : MonsterAction
{
    public BuffAttackersAction(int minimumStrength, int maximumStrength, EventReference sfx) : base(minimumStrength, maximumStrength, sfx)
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will buff enemies that want to attack";
        ActionIntentType = IntentType.Buff;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        foreach (Monster enemy in BoardStateController.getLivingEnemies())
        {
            if (enemy.GetIntent().ActionSelected.ActionIntentType == IntentType.Attack)
            {
                enemy.Buff(strengh);
                Monster _enemy = source as Monster;
                if(_enemy)
                {
                    SoundPlayer.PlaySound(ActionSFX, _enemy.gameObject);
                }
                else
                    SoundPlayer.PlaySound(ActionSFX, null);
            }
        }
    }
}