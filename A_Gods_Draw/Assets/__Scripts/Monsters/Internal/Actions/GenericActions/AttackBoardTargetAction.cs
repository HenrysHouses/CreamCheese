using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoardTargetAction : MonsterAction
{

    public AttackBoardTargetAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will attack the player";

    }

    public override void Execute(BoardStateController _board, int _strength, Object _source = null)
    {
        
        if(_board.ExtraEnemyTargets.Count <= 0)
            return;

        _board.ExtraEnemyTargets[Random.Range(0, _board.ExtraEnemyTargets.Count)].TakeDamage(_strength);

        Monster _enemy = _source as Monster;
        if(_enemy)
            _enemy.animator.SetTrigger("Attack");

    }

}
