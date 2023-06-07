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

            if(_action.Weigth <= 0)
                continue;

            bool _allNeeded = _action.AllRequired, _passed = false;
            for(int j = 0; j < _action.ActionConditions.Length; j++)
            {

                _passed = false;
                
                //Debug.Log("Checking condition " + _action.ActionConditions[j].Condition + " is invert " + _action.ActionConditions[j].InvertCondition);
                SwitchOnCondition(_action.ActionConditions[j].Condition, ref _passed, _board, _intent, _action.Action, _action.ActionConditions[j].InvertCondition);

                if(!_passed && _allNeeded)
                {

                    _possibleActions.Remove(i);
                    break;
                    
                }

                if(!_passed)
                    continue;

                AddAction(ref _possibleActions, i);

            }

            if(!_passed)
                continue;
            //Debug.Log("Action passed: " + _action.ActionType + " prio: " + _action.Priority);
            if(_action.Priority > _highestPrio)
                _highestPrio = _action.Priority;

        }
        //Debug.Log("Posacts: " + _possibleActions.Count);
        if(_possibleActions.Count > 1)
            for(int i = 0; i < _possibleActions.Count; i++)
                if(_actions[_possibleActions[i]].Priority < _highestPrio)
                    _possibleActions.RemoveAt(i);
        //Debug.Log("Posacts after: " + _possibleActions.Count);

        /*for(int i= 0; i < _possibleActions.Count; i++)
            Debug.Log("Possible actions within prio level: " + _actions[_possibleActions[i]].ActionType + " | prio: " + _actions[_possibleActions[i]].Priority);*/
        
        if(_possibleActions.Count > 1)
        {

            int _maxRange = 0;
            for(int i = 0; i < _possibleActions.Count; i++)
            {

                //Debug.Log("Added weigth for action: " + _actions[_possibleActions[i]].ActionType);
                _maxRange += _actions[_possibleActions[i]].Weigth;

            }

            int _rnd = Random.Range(0, _maxRange);
            int _checkVal = 0;
            for(int i = 0; i < _possibleActions.Count; i++)
            {

                _checkVal += _actions[_possibleActions[i]].Weigth;
                /*Debug.Log("Random num was: " + _rnd + "| and Check value was: " + _checkVal + "| added weigth was: " + _actions[_possibleActions[i]].Weigth + "| PRIO: " +_actions[_possibleActions[i]].Priority +
                " for action: " + _actions[_possibleActions[i]].ActionType + " i = " + _possibleActions[i] + "| Enemy: " + _intent.Self.gameObject.name);*/

                if(_rnd < _checkVal)
                    return _actions[_possibleActions[i]].Action;

            }

        }
        else if(_possibleActions.Count == 1)
            return _actions[_possibleActions[0]].Action;

        return null;

    }

    public static bool CheckConditions(ActionCondition[] _conditions, bool _allConditionsNeeded, BoardStateController _board, Intent _intent)
    {

        List<int> _possibleActions = new List<int>();

        if(_conditions.Length == 0)
            return false;
            
        bool _passed = false;
        for(int i = 0; i < _conditions.Length; i++)
        {

            _passed = false;
            
            SwitchOnCondition(_conditions[i].Condition, ref _passed, _board, _intent, null, _conditions[i].InvertCondition);

            if(!_passed && _allConditionsNeeded)
                return false;

            if(!_passed)
                continue;

            return true;

        }

        return false;

    }

    private static void SwitchOnCondition(ActionConditions _condition, ref bool _passed, BoardStateController _board, Intent _intent, MonsterAction _action = null, bool _invertCondition = false)
    {

        switch(_condition)
        {

            case ActionConditions.None:
            _passed = true;
            break;

            case ActionConditions.LastAlive:
            if(_board.getLivingEnemies().Length == 1)
            {
                _passed = true;
            }
            break;

            case ActionConditions.GodPlayed:
            if(_board.isGodPlayed)
            {
                _passed = true;
            }
            break;

            case ActionConditions.HasNotDefended:
            if(!_intent.DefendedLastTurn())
            {
                _passed = true;
            }
            break;

            case ActionConditions.HasNotAttacked:
            if(!_intent.AttackedLastTurn())
            {
                _passed = true;
            }
            break;

            case ActionConditions.HasNotActed:
            if(!_intent.DidActionLastTurn())
            {
                _passed = true;
            }
            break;

            case ActionConditions.PlayerHealthAt50:
            if(_board.Player.Playerhealth < Mathf.RoundToInt(_board.Player.MaxPlayerHealth / 2))
            {
                _passed = true;
            }
            break;

            case ActionConditions.NotSameAction:
            if(!_intent.SameAction(_action))
            {
                _passed = true;
            }
            break;

            case ActionConditions.ExtraTargetsOnBoard:
            if(_board.ActiveExtraEnemyTargets != null)
            {
                _passed = true;
            }
            break;

            case ActionConditions.SelfHealthAtHalf:
            if(_intent.Self.GetHealth() < Mathf.RoundToInt(_intent.Self.GetMaxHealth() / 2))
            {
                _passed = true;
            }
            break;

            case ActionConditions.SelfHealthAtQuarter:
            if(_intent.Self.GetHealth() < Mathf.RoundToInt(_intent.Self.GetMaxHealth() / 4))
            {
                _passed = true;
            }
            break;

            case ActionConditions.HasNotBuffed:
            if(!_intent.BuffedLastTurn())
            {
                _passed = true;
            }
            break;

            case ActionConditions.AnyEnemyHealthHalf:
            foreach (Monster _enemy in _board.getLivingEnemies())
            {
                if(_enemy.GetHealth() > _enemy.GetMaxHealth() / 2)
                    continue;
                
                _passed = true;
                break;
            }
            break;

            case ActionConditions.AnyEnemyHealthQuarter:
            foreach (Monster _enemy in _board.getLivingEnemies())
            {
                if(_enemy.GetHealth() > _enemy.GetMaxHealth() / 4)
                    continue;
                
                _passed = true;
                break;
            }
            break;

            case ActionConditions.AnyEnemyDamaged:
            foreach (Monster _enemy in _board.getLivingEnemies())
            {
                if(_enemy.GetHealth() >= _enemy.GetMaxHealth())
                    continue;
                
                _passed = true;
                break;
            }
            break;

            case ActionConditions.AnyEnemyDebuffed:
            foreach (Monster _enemy in _board.getLivingEnemies())
            {
                if(!_enemy.HasDebuffNextRound())
                    continue;
                
                _passed = true;
                break;
            }
            break;

            case ActionConditions.IsBuffed:
            if(_intent.Self.BuffStrength > 0)
            {
                _passed = true;
            }
            break;

            case ActionConditions.FenrirBeaten1:
            if(GameManager.timesDefeatedBoss >= 1)
            {
                _passed = true;
            }
            break;

            case ActionConditions.FenrirBeaten2:
            if(GameManager.timesDefeatedBoss >= 2)
            {
                _passed = true;
            }
            break;

            case ActionConditions.FenrirBeaten3:
            if(GameManager.timesDefeatedBoss >= 3)
            {
                _passed = true;
            }
            break;

            case ActionConditions.FenrirBoardTargetCheck:
            if(_board.ActiveExtraEnemyTargets.Count > Random.Range(0, _board.AllExtraEnemyTargets.Count))
            {
                _passed = true;
            }
            break;

            case ActionConditions.HasNotPerformedSpecialAction:
            if(!_intent.SpecialActionLastTurn())
            {
                _passed = true;
            }
            break;

        }

        if(_invertCondition)
            _passed = !_passed;

    }

    private static void AddAction(ref List<int> _actions, int _i)
    {

        if(!_actions.Contains(_i))
            _actions.Add(_i);

    }

}
