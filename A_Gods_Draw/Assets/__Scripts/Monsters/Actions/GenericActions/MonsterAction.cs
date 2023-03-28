using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MonsterAction : Action
{
    public MonsterAction(int minimumStrength, int maximumStrength)
    {
        MinStrength = minimumStrength;
        MaxStrength = maximumStrength;
        Targets = new List<IMonsterTarget>();
    }

    public int MinStrength, MaxStrength;
    public List<IMonsterTarget> Targets;
}
