using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Intent
{
    public EnemyIntent GetID()
    {
        EnemyIntent _i = (EnemyIntent)actionSelected.ID;
        return _i;
    }

    protected int strengh;

    protected Action actionSelected;

    public void CancelIntent()
    {
        actionSelected = null;
    }

    public abstract void DecideIntent(BoardStateController board);

    public int GetCurrStrengh() => strengh;

    public void Act(BoardStateController BoardStateController)
    {
        if (actionSelected.ID == (int)EnemyIntent.None)
            return;

        actionSelected.Execute(BoardStateController, strengh);

        CancelIntent();
    }
}

public enum EnemyIntent
{
    Buff,
    Defend,
    AttackGod,
    AttackPlayer,
    None,
}