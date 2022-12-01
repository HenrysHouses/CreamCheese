using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action
{
    protected int ActionID; // Ex: constructor -> ActionID = EnemyIntent.Attack;
    public int ID => this != null ? ActionID : (int)EnemyIntent.None;

    protected int min = 0, max = 3;

    protected bool isReady = false;

    protected Sprite actionIcon;
    public Sprite Icon => actionIcon;

    protected string desc = "";
    public string Explanation => desc;

    public Action(int _min, int _max)
    {
        min = _min;
        max = _max;
    }

    public int Min() => min;
    public int Max() => max;

    public void Act(BoardStateController BoardStateController, int strengh, UnityEngine.Object source = null)
    {
        isReady = false;
        Execute(BoardStateController, strengh, source);
    }
    public abstract void Execute(BoardStateController board, int strengh, UnityEngine.Object source = null);

    public static bool operator!(Action action)
    {
        return action == null;
    }
}
