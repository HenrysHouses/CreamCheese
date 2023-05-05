using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

public class IdlingAction : MonsterAction
{

    public IdlingAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.None;
        actionIcon = Resources.Load<Sprite>("ImageResources/Icon_ZZZ_IMG_v1");
        desc = "This enemy is waiting for this turn";
        ActionIntentType = IntentType.Idling;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {}

}
