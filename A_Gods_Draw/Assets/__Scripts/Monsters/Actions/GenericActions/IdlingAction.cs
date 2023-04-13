using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class IdlingAction : MonsterAction
{

    public IdlingAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy is waiting for this turn";
        ActionIntentType = IntentType.Idling;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
    }

}
