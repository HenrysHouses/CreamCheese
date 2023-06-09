using System.Collections.Generic;
using UnityEngine;
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

                case EnemyIntent.Attack_Defend:
                _newAction = new Attack_DefendAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.AttackGod_Defend:
                _newAction = new AttackGod_DefendAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.Attack_Heal:
                _newAction = new Attack_HealAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.Cleanse_Buff:
                _newAction = new Cleanse_BuffAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.Cleanse_Heal:
                _newAction = new Cleanse_HealAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;
                
                case EnemyIntent.Heal_Buff:
                _newAction = new Heal_BuffAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
                break;

                case EnemyIntent.BuffAll:
                _newAction = new MultiBuffEnemyAction(actions[i].MinStrength + _scale, actions[i].MaxStrength + _scale);
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

    public override bool SpecialActionLastTurn()
    {
        if(PreviousAction != null && PreviousAction.ActionIntentType == IntentType.Special)
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

    public override void LateIntentUpdate(BoardStateController _board)
    {

        if(actionSelected.IsLocked)
        {

            if(Self.Weakened)
                strength += Self.BuffStrength;
            else
                strength += Self.BuffStrength / 2;

            return;

        }

        if(actionSelected != idleAction)
        {

            strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1) + Self.BuffStrength;

            if(actionSelected.ActionSettings.UseStrengthMod)
                if(ConditionChecker.CheckConditions(actionSelected.ActionSettings.StrengthModConditions, actionSelected.ActionSettings.AllRequiredForMod, _board, this))
                    strength += actionSelected.ActionSettings.ModifiedStrength;

            if(Self.Weakened)
            {
                strength = (int)Mathf.Clamp(strength / 2, 1, Mathf.Infinity);
                Debug.Log("Is Weakened yessus good stuff poggies");
            }

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

namespace EnemyAIEnums
{

    public enum ActionConditions
    {
        None = 0,
        LastAlive,
        GodPlayed,
        HasNotDefended,
        HasNotAttacked,
        HasNotActed,
        PlayerHealthAt50,
        NotSameAction,
        ExtraTargetsOnBoard,
        SelfHealthAtHalf,
        SelfHealthAtQuarter,
        HasNotBuffed,
        AnyEnemyHealthHalf,
        AnyEnemyHealthQuarter,
        AnyEnemyDamaged,
        AnyEnemyDebuffed,
        IsBuffed,
        FenrirBeaten1,
        FenrirBeaten2,
        FenrirBeaten3,
        FenrirBoardTargetCheck,
        HasNotPerformedSpecialAction,
    }

    public enum IntentType
    {
        Attack,
        Defend,
        Buff,
        Special,
        Idling
    }

    public enum EnemyIntent
    {
        BuffEnemy = 0,
        Defend = 1,
        AttackGod = 2,
        AttackPlayer = 3,
        None = 5,
        AttackExtraTarget = 4,
        DoubleAttack = 6,
        FenrirDoubleAttack = 7,
        ReinforceSelf = 8,
        HealEnemy = 9,
        CleanseEnemy = 10,
        BuffSelf = 11,
        Attack_Defend = 12,
        AttackGod_Defend = 13,
        Attack_Heal = 14,
        Cleanse_Buff = 15,
        Cleanse_Heal = 16,
        Heal_Buff = 17,
        SummonEnemies = 18,
        BuffAll = 19

    }

}