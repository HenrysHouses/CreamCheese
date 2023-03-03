using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionChecker
{

    public static MonsterAction CheckConditions(ActionSelection[] _actions)
    {

        List<int> _possibleActions = new List<int>();

        if(_actions.Length == 0)
            return null;

        foreach (ActionSelection _action in _actions)
        {

            switch (_action.ActionCondition[0])
            {
            
                case Conditions.GodPlayed:

                break;
                
            }
            
        }

        return null;

    }

}
