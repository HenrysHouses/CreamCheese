// Written by Javier

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MinionIntent : Intent
{

    private ActionSelection[] actions;
    private IdlingAction idleAction;

    public MinionIntent(ref ActionSelection[] _actions, Monster _self)
    {

        Self = _self;
        idleAction = new IdlingAction(0, 0);
        actions = _actions;
        int _scale = GameManager.timesDefeatedBoss;

        for(int i = 0; i < actions.Length; i++)
        {

            MonsterAction _newAction = null;

            switch(actions[i].ActionType)
            {

                case EnemyIntent.BuffAttackers:
                _newAction = new BuffAttackersAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.AttackGod:
                _newAction = new AttackGodAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.AttackPlayer:
                _newAction = new AttackPlayerAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.Defend:
                _newAction = new DefendAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.AttackExtraTarget:
                _newAction = new AttackBoardTargetAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.DoubleAttack:
                _newAction = new DoubleAttackAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.FenrirDoubleAttack:
                _newAction = new FenrirDoubleAttackAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

            }

            actions[i].Action = _newAction;
            _newAction.Self = _self;
            _newAction.ActionSFX = actions[i].ActionSFX;
            _newAction.TurnsToPerform = actions[i].TurnsToPerform;

        }

    }

    public T GetAction<T>() where T : Action
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if(actions[i].Action is T)
                return actions[i].Action as T;
        }
        return null;
    }

    public override bool DefendedLastTurn()
    {

        if(PreviousAction != null && PreviousAction.ActionIntentType == IntentType.Defend)
            return true;
        
        return false;

    }

    public override bool AttackedLastTurn()
    {
        if(PreviousAction != null && PreviousAction.ActionIntentType == IntentType.Attack)
            return true;

        return false;
    }

    public override bool DidActionLastTurn()
    {
        if(PreviousAction != null && PreviousAction.ActionIntentType != IntentType.Idling)
            return true;

        return false;
    }

    public override bool BuffedLastTurn()
    {
        if(PreviousAction != null && PreviousAction.ActionIntentType == IntentType.Buff)
            return true;

        return false;
    }

    public override void CancelIntent()
    {
        if(actionSelected != null)
            actionSelected.IsLocked = false;
        actionSelected = idleAction;
        strength = 0;
    }

    public override void DecideIntent(BoardStateController _board)
    {

        if(actionSelected != null && actionSelected.IsLocked)
            return;
        if(actionSelected != null)
            Debug.Log("choosing new action, " + actionSelected.TurnsLeft + " turns left on action, previous action " + actionSelected.GetType());

        actionSelected = ConditionChecker.CheckConditions(actions, _board, this);
        actionSelected.TurnsLeft = actionSelected.TurnsToPerform;

    }
    public override void LateDecideIntent(BoardStateController _board)
    {

        if(actionSelected.IsLocked)
            return;

        if (actionSelected != null)
            strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1) + Self.BuffStrength;
        else
            actionSelected = idleAction;

        PreviousAction = actionSelected;
        actionSelected.SelectTargets(_board);

    }
}