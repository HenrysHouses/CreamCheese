using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAction : Action
{
    public IMonster toDefend;

    public DefendAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.Defend;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh)
    {
        throw new System.NotImplementedException();
    }
}
