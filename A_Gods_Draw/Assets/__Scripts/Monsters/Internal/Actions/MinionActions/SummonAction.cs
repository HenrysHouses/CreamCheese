using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAction : MonsterAction
{

    public SummonAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.SummonEnemies;
        Debug.LogError("This intent needs an image");
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will summon other enemies next turn";

    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {

        //who knows...

    }

}
