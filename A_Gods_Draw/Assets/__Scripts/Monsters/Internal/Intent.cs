using System.Collections;
using System.Collections.Generic;
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

    protected int strengh;

    protected Action actionSelected;

    public void CancelIntent()
    {
        actionSelected = null;
        strengh = 0;
    }

    public abstract void DecideIntent(BoardStateController board);

    public virtual void LateDecideIntent(BoardStateController board) { }

    public int GetCurrStrengh() => strengh;
    public int SetCurrStrengh(int newS) => strengh = newS;

    public void Act(BoardStateController board)
    {
        if (actionSelected != null)
        {
            if (actionSelected.ID == (int)EnemyIntent.None)
                return;

            actionSelected.Execute(board, strengh);
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