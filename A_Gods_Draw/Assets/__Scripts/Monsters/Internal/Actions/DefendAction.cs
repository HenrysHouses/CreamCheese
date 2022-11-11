using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAction : Action
{
    public IMonster toDefend;

    public DefendAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.Defend;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/21");
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        toDefend.Defend(strengh);
    }
}
