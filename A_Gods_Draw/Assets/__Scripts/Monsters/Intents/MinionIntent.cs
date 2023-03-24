// Written by Javier

using System.Collections.Generic;
using UnityEngine;

public class MinionIntent : Intent
{

    //List<Action> Actions = new List<Action>();
    private ActionSelection[] actions;
    private IdlingAction idleAction;

    public MinionIntent(ref ActionSelection[] _actions)
    {

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

            }

            actions[i].Action = _newAction;

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

        if(PreviousAction.ActionIntentType == IntentType.Defend)
            return true;
        
        return false;

    }

    public override bool AttackedLastTurn()
    {
        if(PreviousAction.ActionIntentType == IntentType.Attack)
            return true;

        return false;
    }

    public override bool DidActionLastTurn()
    {
        if(PreviousAction.ActionIntentType != IntentType.Idling)
            return true;

        return false;
    }

    public override void CancelIntent()
    {
        actionSelected = idleAction;
        strength = 0;
    }

    public override void DecideIntent(BoardStateController _board)
    {

        actionSelected = ConditionChecker.CheckConditions(actions, _board, this);

    }
    public override void LateDecideIntent(BoardStateController _board)
    {

        if (actionSelected != null)
            strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1);
        else
            actionSelected = idleAction;

        if(GetAction<DefendAction>() != null && actionSelected == GetAction<DefendAction>())
            GetAction<DefendAction>().toDefend = Self;

        PreviousAction = actionSelected;

    }
}