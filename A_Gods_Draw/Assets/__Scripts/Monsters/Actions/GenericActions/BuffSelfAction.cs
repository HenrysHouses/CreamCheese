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

    public override IEnumerator PerformAction(BoardStateController _board, int _strength, object _source)
    {
        Self.Strengthen(_strength);
        if(ActionSettings.ActionVFX)
            GameObject.Instantiate(ActionSettings.ActionVFX, Self.transform.position, Quaternion.identity);
        
        yield return null;

    }
}