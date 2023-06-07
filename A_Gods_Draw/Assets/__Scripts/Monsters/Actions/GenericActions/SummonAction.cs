using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class SummonAction : MonsterAction
{
    private Vector3 avaviblePosition, currentMonsterPossies;
    

    public SummonAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.SummonEnemies;
        Debug.LogError("This intent needs an image");
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will summon other enemies next turn";

    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {


        //who knows...

    }

    public override void SelectTargets(BoardStateController _board)
    {
        
        //currentMonsterPossies
        
    }

}
