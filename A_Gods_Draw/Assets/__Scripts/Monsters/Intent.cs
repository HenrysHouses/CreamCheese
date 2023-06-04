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
    public int GetStrength => strength;
    
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
    public virtual void LateIntentUpdate(BoardStateController _board){}
    public virtual bool DefendedLastTurn(){return false;}
    public virtual bool AttackedLastTurn(){return false;}
    public virtual bool DidActionLastTurn(){return false;}
    public virtual bool BuffedLastTurn(){return false;}
    public virtual bool SameAction(MonsterAction _action){return false;}
    public virtual ActionSelection TryGetActionSelectionForType(EnemyIntent _intentType){return null;}

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

    public MonsterAction Act(BoardStateController _board, UnityEngine.Object _source)
    {
        if (actionSelected != null)
        {
            if (actionSelected.ID == (int)EnemyIntent.None)
                return null;

            actionSelected.Execute(_board, strength, _source);
        }
        return actionSelected;
    }
}

