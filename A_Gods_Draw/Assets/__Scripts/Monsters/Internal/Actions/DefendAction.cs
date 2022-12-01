using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAction : Action
{
    public IMonster toDefend;

    public DefendAction(int _min, int _max) : base(_min, _max)
    {
        ActionID = (int)EnemyIntent.Defend;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Shield_IMG_v1");
        desc = "This enemy will defend an enemy for the next turn onward";
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        if (toDefend)
            toDefend.Defend(strengh);
    }
}
