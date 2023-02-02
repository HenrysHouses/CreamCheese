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

    protected int _min = 0, _max = 3;
    public int MinStrength => _min;
    public int MaxStrength => _min;
}
