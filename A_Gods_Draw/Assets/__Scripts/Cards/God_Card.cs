using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/God card")]

public class God_Card : Card_SO
{
    public short health;

    public IGodActions god;
    public override Card_Behaviour Init(GameObject a)
    {
        God_Behaviour behaviour = a.AddComponent<God_Behaviour>();
        behaviour.Initialize(this);
        return behaviour;
    }
}
