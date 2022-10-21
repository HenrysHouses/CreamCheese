using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAttackersAction : Action
{
    public BuffAttackersAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.BuffAttackers;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh)
    {
        throw new System.NotImplementedException();
    }
}