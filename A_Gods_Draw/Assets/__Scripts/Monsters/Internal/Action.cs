using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action
{
    protected int ActionID; // Ex: constructor -> ActionID = EnemyIntent.Attack;
    public int ID => this != null ? ActionID : (int)EnemyIntent.None;

    protected int min = 0, max = 3;

    public Action(int _min, int _max)
    {
        min = _min;
        max = _max;
    }

    public int Min() => min;
    public int Max() => max;

    public abstract void Execute(BoardStateController BoardStateController, int strengh);

    public static bool operator!(Action action)
    {
        return action != null;
    }
    public static bool operator ==(Action action, EnemyIntent intent)
    {
        return action.ID == (int)intent;
    }
    public static bool operator !=(Action action, EnemyIntent intent)
    {
        return action.ID != (int)intent;
    }
}
