using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

[System.Serializable]
public abstract class MonsterAction : Action
{
    public MonsterAction(int minimumStrength, int maximumStrength)
    {
        MinStrength = minimumStrength;
        MaxStrength = maximumStrength;
        Targets = new List<IMonsterTarget>();
    }

    public int MinStrength, MaxStrength, TurnsToPerform, TurnsLeft;
    public List<IMonsterTarget> Targets;
    public EventReference ActionSFX;
    public Monster Self;
    public bool IsLocked;
    public ActionSelection ActionSettings;
    public override void Execute(BoardStateController _board, int _strength, Object _source = null)
    {

        if(TurnsToPerform == 0)
        {
            PerformAction(_board, _strength, _source);
            return;
        }

        if(TurnsLeft <= 0)
        {
            PerformAction(_board, _strength, _source);
            IsLocked = false;
            return;
        }

        TurnsLeft -= 1;
        IsLocked = true;

    }

    public virtual void PerformAction(BoardStateController _board, int _strength, object _source)
    {}

}
