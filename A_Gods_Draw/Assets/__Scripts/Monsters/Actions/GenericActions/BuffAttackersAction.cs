// Written by Javier

using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

public class BuffAttackersAction : MonsterAction
{
    public BuffAttackersAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will buff a random enemy";
        ActionIntentType = IntentType.Buff;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {
        Monster[] _monsters = _board.getLivingEnemies();

        _monsters[Random.Range(0, _monsters.Length)].Buff(_strength);
    }
}