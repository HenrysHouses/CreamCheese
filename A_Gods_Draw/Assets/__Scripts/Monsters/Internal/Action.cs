using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action
{
    protected int ActionID; // Ex: constructor -> ActionID = EnemyIntent.Attack;
    public int ID => ActionID;
    
    public abstract void Execute();
}
