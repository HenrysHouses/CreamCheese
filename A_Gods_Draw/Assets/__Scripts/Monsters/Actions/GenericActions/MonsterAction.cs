using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public abstract class MonsterAction : Action
{
    public MonsterAction(int minimumStrength, int maximumStrength, EventReference sfx)
    {
        MinStrength = minimumStrength;
        MaxStrength = maximumStrength;
        ActionSFX = sfx;
        Targets = new List<IMonsterTarget>();
    }

    public int MinStrength, MaxStrength;
    public List<IMonsterTarget> Targets;
    public EventReference ActionSFX;
}
