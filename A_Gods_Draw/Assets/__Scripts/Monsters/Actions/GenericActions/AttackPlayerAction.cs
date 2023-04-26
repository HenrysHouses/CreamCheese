// Written by Javier

using UnityEngine;
using FMODUnity;
using EnemyAIEnums;
using System.Collections;

[System.Serializable]
public class AttackPlayerAction : MonsterAction
{
    public AttackPlayerAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the player";
        ActionIntentType = IntentType.Attack;
    }

    public override IEnumerator PerformAction(BoardStateController _board, int _strength, object _source)
    {
        
        _board.Player.DealDamage(_strength);

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
        }

        yield return null;

    }
}
