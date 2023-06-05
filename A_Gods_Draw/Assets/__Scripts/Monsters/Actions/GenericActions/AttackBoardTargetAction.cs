using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

public class AttackBoardTargetAction : MonsterAction
{

    public AttackBoardTargetAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will attack a target on the board";
        ActionIntentType = IntentType.Attack;

    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
        
        if(_board.ActiveExtraEnemyTargets.Count <= 0)
            return;

        _board.ActiveExtraEnemyTargets[Random.Range(0, _board.ActiveExtraEnemyTargets.Count)].DealDamage(_strength);

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.Animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
        }
        
    }

}
