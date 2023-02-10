// Written by Javier

using UnityEngine;

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

    protected MonsterAction actionSelected;

    public void CancelIntent()
    {
        actionSelected = null;
        strength = 0;
    }

    public abstract void DecideIntent(BoardStateController board);

    public virtual void LateDecideIntent(BoardStateController board) { }

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

    public void Act(BoardStateController board, UnityEngine.Object source)
    {
        if (actionSelected != null)
        {
            if (actionSelected.ID == (int)EnemyIntent.None)
                return;

            actionSelected.Execute(board, strength, source);
        }

        CancelIntent();
    }
}

public enum EnemyIntent
{
    Buff,
    BuffAttackers,
    Defend,
    AttackGod,
    AttackPlayer,
    None = 5
}