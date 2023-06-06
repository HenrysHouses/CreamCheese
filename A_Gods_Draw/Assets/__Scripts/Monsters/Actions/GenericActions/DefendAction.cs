// Written by Javier

using UnityEngine;
using FMODUnity;
using EnemyAIEnums;
using System.Collections;

public class DefendAction : MonsterAction
{

    public DefendAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.Defend;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Shield_IMG_v1");
        desc = "This enemy will defend itself next turn";
        ActionIntentType = IntentType.Defend;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
        
        Self.Defend(_strength);

        Monster _enemy = _source as Monster;
        if(_enemy)
            _enemy.PlaySound(ActionSFX);

    }

}
