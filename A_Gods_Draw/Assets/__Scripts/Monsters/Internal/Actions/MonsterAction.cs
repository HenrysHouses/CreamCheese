using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAction : Action
{
    public MonsterAction(int minimumStrength, int maximumStrength)
    {
        _min = minimumStrength;
        _max = maximumStrength;
    }

    protected int _min, _max;
    public int MinStrength => _min;
    public int MaxStrength => _max;
}
