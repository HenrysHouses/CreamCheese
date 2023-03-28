/* 
 * Written by Henrik
 * Edited by Javier
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action
{
    protected int ActionID; // Ex: constructor -> ActionID = EnemyIntent.Attack;
    public int ID => this != null ? ActionID : (int)EnemyIntent.None;
    public IntentType ActionIntentType;

    protected bool isReady = false;
    public bool Ready => isReady;

    protected Sprite actionIcon;
    public Sprite Icon => actionIcon;

    protected string desc = "";
    public string Explanation => desc;

    public void Act(BoardStateController BoardStateController, int strengh, UnityEngine.Object source = null)
    {
        isReady = false;
        Execute(BoardStateController, strengh, source);
    }
    public abstract void Execute(BoardStateController board, int strengh, UnityEngine.Object source = null);
    public virtual void SelectTargets(BoardStateController _board)
    {}
}
