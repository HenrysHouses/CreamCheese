using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGodAction : Action
{
    public AttackGodAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.AttackGod;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh)
    {
        throw new System.NotImplementedException();
    }
}
