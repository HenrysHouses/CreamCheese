using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionChecker
{

    public static MonsterAction CheckConditions(ActionSelection[] _actions, BoardStateController _board, Intent _monster)
    {

        List<int> _possibleActions = new List<int>();

        if(_actions.Length == 0)
            return null;

        int _highestPrio = 0;
        for(int i = 0; i < _actions.Length; i++)
        {

            ActionSelection _action = _actions[i];
            bool _allNeeded = _action.AllRequired, _passed;
            for(int j = 0; j < _action.ActionConditions.Length; j++)
            {

                _passed = false;
                switch(_action.ActionConditions[j])
                {

                    case Conditions.None:
                    AddAction(ref _possibleActions, i);
                    _passed = true;
                    break;

                    case Conditions.LastAlive:
                    if(_board.getLivingEnemies().Length == 1)
                    {
                        AddAction(ref _possibleActions, i);
                        _passed = true;
                    }
                    break;

                    case Conditions.GodPlayed:
                    if(_board.isGodPlayed)
                    {
                        AddAction(ref _possibleActions, i);
                        _passed = true;
                    }
                    break;

                    case Conditions.HasNotDefended:
                    if(_monster.DefendedLastTurn())
                    {
                        AddAction(ref _possibleActions, i);
                        _passed = true;
                    }
                    break;

                }

                if(!_passed && _allNeeded)
                {

                    _possibleActions.Remove(i);
                    break;
                    
                }

                if(!_passed)
                    continue;

                if(_action.Priority > _highestPrio)
                    _highestPrio = _action.Priority;

            }

        }

        if(_possibleActions.Count > 1)
            for(int i = 0; i < _possibleActions.Count; i++)
                if(_actions[_possibleActions[i]].Priority < _highestPrio)
                    _possibleActions.RemoveAt(i);
        
        if(_possibleActions.Count > 1)
        {

            int _maxRange = 0;
            for(int i = 0; i < _possibleActions.Count; i++)
            {

                _maxRange += _actions[_possibleActions[i]].Weigth;

            }

            int _rnd = Random.Range(0, _maxRange);
            int _checkVal = 0;
            for(int i = 0; i < _possibleActions.Count; i++)
            {

                _checkVal += _actions[_possibleActions[i]].Weigth;

                if(_rnd < _checkVal)
                    return _actions[_possibleActions[i]].Action;

            }

        }
        else if(_possibleActions.Count == 1)
            return _actions[_possibleActions[0]].Action;

        Debug.Log(@"Failed to find a valid action, make sure you have a |None| condition action set");
        return null;

    }

    private static void AddAction(ref List<int> _actions, int _i)
    {

        if(!_actions.Contains(_i))
            _actions.Add(_i);

    }

}
