// Written by Javier

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

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

                case EnemyIntent.BuffEnemy:
                _newAction = new BuffEnemyAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
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

                case EnemyIntent.ReinforceSelf:
                _newAction = new ReinforceSelfAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.HealEnemy:
                _newAction = new HealEnemyAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.CleanseEnemy:
                _newAction = new CleanseEnemyAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.BuffSelf:
                _newAction = new BuffSelfAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

            }

            actions[i].Action = _newAction;
            _newAction.Self = _self;
            _newAction.ActionSFX = actions[i].ActionSFX;
            _newAction.TurnsToPerform = actions[i].TurnsToPerform;
            _newAction.TurnsLeft = actions[i].TurnsToPerform;
            _newAction.ActionSettings = actions[i];

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

    public override bool SameAction(MonsterAction _action)
    {
        if(_action == null)
        {
            if(actionSelected != PreviousAction)
                return true;
        }
        else
            if(_action == PreviousAction)
                return true;

        return false;
    }

    public override void CancelIntent()
    {
        if(actionSelected != null)
        {
            actionSelected.IsLocked = false;
            actionSelected.TurnsLeft = actionSelected.TurnsToPerform;
        }
        actionSelected = idleAction;
        strength = 0;
    }

    public override void DecideIntent(BoardStateController _board)
    {

        if(actionSelected != null && actionSelected.IsLocked)
            return;
        if(actionSelected != null)
            PreviousAction = actionSelected;

        actionSelected = ConditionChecker.CheckConditions(actions, _board, this);
        if(actionSelected == null)
            actionSelected = idleAction;

    }
    public override void LateDecideIntent(BoardStateController _board)
    {

        if(actionSelected.IsLocked)
        {

            strength += Self.BuffStrength;
            return;

        }

        if(actionSelected != idleAction)
        {

            if(actionSelected.ActionSettings.UseStrengthMod)
            {

                if(ConditionChecker.CheckConditions(actionSelected.ActionSettings.ActionConditions, actionSelected.ActionSettings.AllRequiredForMod, _board, this))
                    strength = actionSelected.ActionSettings.ModifiedStrength + Self.BuffStrength;
                else
                    strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1) + Self.BuffStrength;

            }
            else
                strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1) + Self.BuffStrength;

        }

        for(int i = 0; i < actions.Length; i++)
        {

            if(!actions[i].UseWeigthMod)
                continue;

            if(ConditionChecker.CheckConditions(actions[i].WeigthModConditions, actions[i].AllRequiredForWeigthMod, _board, this))
            {

                actions[i].Weigth += actions[i].WeigthMod;
                actions[i].AddedWeigth += actions[i].WeigthMod;
                continue;

            }

            if(actions[i].ClearOnConditionFalse && actions[i].AddedWeigth != 0)
            {

                actions[i].Weigth -= actions[i].AddedWeigth;
                actions[i].AddedWeigth = 0;

            }

        }

        actionSelected.SelectTargets(_board);

    }
}