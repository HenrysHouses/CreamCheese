using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class IdlingAction : MonsterAction
{

    public IdlingAction(int minimumStrength, int maximumStrength, EventReference sfx) : base(minimumStrength, maximumStrength, sfx)
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy is waiting for this turn";
        ActionIntentType = IntentType.Idling;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
    }

}
