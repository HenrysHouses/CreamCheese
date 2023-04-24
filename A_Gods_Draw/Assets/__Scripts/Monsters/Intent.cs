// Written by Javier

using UnityEngine;
using EnemyAIEnums;

public abstract class Intent
{
    public EnemyIntent GetID()
    {
        if(actionSelected is null)
            return (EnemyIntent)5;

        EnemyIntent _i = (EnemyIntent)actionSelected.ID;
        return _i;
    }

    protected int strength;
    
    public Monster Self;
    protected MonsterAction actionSelected;
    public MonsterAction ActionSelected
    {
        get {return actionSelected;}
    }
    public MonsterAction PreviousAction;

    public abstract void CancelIntent();
    public abstract void DecideIntent(BoardStateController _board);

    public virtual void TutorialIntentOverride(BoardStateController _board, TutorialActions _actionToPerform){}
    public virtual void LateDecideIntent(BoardStateController _board){}
    public virtual bool DefendedLastTurn(){return false;}
    public virtual bool AttackedLastTurn(){return false;}
    public virtual bool DidActionLastTurn(){return false;}
    public virtual bool BuffedLastTurn(){return false;}

    public int GetCurrStrengh() => strength;
    public int SetCurrStrengh(int newS) => strength = newS;
    public Sprite GetCurrentIcon()
    {
        return actionSelected?.Icon;
    }
    public string GetCurrentDescription()
    {
        return actionSelected?.Explanation;
    }

    public void Act(BoardStateController _board, UnityEngine.Object _source)
    {
        if (actionSelected != null)
        {
            if (actionSelected.ID == (int)EnemyIntent.None)
                return;

            actionSelected.Execute(_board, strength, _source);
        }
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
        SelfHealthAt50,
        SelfHealthAt25,
        HasNotBuffed,
        AnyEnemyHealth50,
        AnyEnemyHealth25,
        AnyEnemyDamaged,
        AnyEnemyDebuffed
    }

    public enum WeigthConditions
    {
        None,
        ActionNotUsed,
        ActionUsed
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
        BuffAttackers,
        Defend,
        AttackGod,
        AttackPlayer,
        None = 5,
        AttackExtraTarget,
        DoubleAttack,
        FenrirDoubleAttack,
        ReinforceSelf,
        HealEnemy,
        CleanseEnemy
    }

}