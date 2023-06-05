using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class BuffSelfAction : MonsterAction
{
    public BuffSelfAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.BuffSelf;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will buff itself";
        ActionIntentType = IntentType.Buff;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
        
        Self.Buff(_strength);
        if(ActionSettings.ActionVFX)
            GameObject.Instantiate(ActionSettings.ActionVFX, Self.transform.position, Quaternion.identity);

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.Animator.SetTrigger("Buffing");
            _enemy.PlaySound(ActionSFX);
        }

    }
}