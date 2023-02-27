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
    }

    public int MinStrength, MaxStrength;
}
