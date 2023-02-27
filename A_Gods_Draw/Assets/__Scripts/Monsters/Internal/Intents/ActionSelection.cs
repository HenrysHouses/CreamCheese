using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionSelection
{
    
    public monsterActions Action;
    public int MinStrength, MaxStrength;
    //public IntentCondition

    public enum monsterActions
    {

        AttackPlayer,
        AttackGod,
        BuffAttackers,
        Defend

    }

}