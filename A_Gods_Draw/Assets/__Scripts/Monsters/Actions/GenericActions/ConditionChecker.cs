using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public static class ConditionChecker
{

    public static MonsterAction CheckConditions(ActionSelection[] _actions, BoardStateController _board, Intent _intent)
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
                    _passed = true;
                    break;

                    case Conditions.LastAlive:
                    if(_board.getLivingEnemies().Length == 1)
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.GodPlayed:
                    if(_board.isGodPlayed)
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.HasNotDefended:
                    if(!_intent.DefendedLastTurn())
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.HasNotAttacked:
                    if(!_intent.AttackedLastTurn())
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.HasNotActed:
                    if(!_intent.DidActionLastTurn())
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.PlayerHealthAt50:
                    if(_board.Player.Playerhealth < Mathf.RoundToInt(_board.Player.MaxPlayerHealth / 2))
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.NotSameAction:
                    if(!_action.Action.Equals(_intent.PreviousAction))
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.ExtraTargetsOnBoard:
                    if(_board.ActiveExtraEnemyTargets != null)
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.SelfHealthAt50:
                    if(_intent.Self.GetHealth() < Mathf.RoundToInt(_intent.Self.GetMaxHealth() / 2))
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.SelfHealthAt25:
                    if(_intent.Self.GetHealth() < Mathf.RoundToInt(_intent.Self.GetMaxHealth() / 4))
                    {
                        _passed = true;
                    }
                    break;

                    case Conditions.HasNotBuffed:
                    if(!_intent.BuffedLastTurn())
                    {
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

                AddAction(ref _possibleActions, i);

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

        return null;

    }

    private static void AddAction(ref List<int> _actions, int _i)
    {

        if(!_actions.Contains(_i))
            _actions.Add(_i);

    }

}
