using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ReinforceSelfAction : MonsterAction
{

    public ReinforceSelfAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will Reinforce itself giving it 50% more health (Action takes 2 turns)";
        ActionIntentType = IntentType.Special;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
        Self.ApplyBarrier(Mathf.RoundToInt(Self.GetMaxHealth() / 2));

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.PlaySound(ActionSFX);
        }
    }
    
}
