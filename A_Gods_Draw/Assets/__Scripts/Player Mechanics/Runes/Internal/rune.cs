using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class rune
{
    public RuneData RuneData;
    public bool hasTriggered;

    void Reset()
    {
        RuneData.Strength = 0;
        RuneData.State = RuneState.Disabled;   
        hasTriggered = false;
    }

    public virtual void RuneEffect(TurnController controller){}

    public void triggerOnce()
    {
        hasTriggered = true;
    }

    public void resetTrigger()
    {
        hasTriggered = false;
    }

    public rune(int str, RuneState state){}
    public rune(int str){}
    // Virtual overloads if needed for non combat runes
    // protected virtual void RuneEffect(TurnController controller){}
}

[System.Serializable]
public class RuneData
{
    public int ID => (int)Name;
    public RuneState State;
    public int Strength;
    [ReadOnly] public RuneType Name;
    [ReadOnly] public CombatState Trigger;

    public RuneData(RuneType type, CombatState runeTrigger)
    {
        this.Name = type;
        this.Trigger = runeTrigger;
        this.State = RuneState.Disabled;
    }
}

public enum RuneType
{
    UrrStrength = 0,
    FeWealth = 1,
    TursChaos = 2
}

public enum RuneState
{
    Active,
    Disabled,
    Temporary
}