using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class Attack_DefendAction : MonsterAction
{

    public Attack_DefendAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.Attack_Defend;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the player and defend itself";
        ActionIntentType = IntentType.Attack;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
        
        _board.Player.DealDamage(_strength);

        Monster _enemy = _source as Monster;
        if(_enemy)
        {

            _enemy.Animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
            
        }
        Self.Defend(_strength);

    }

}
