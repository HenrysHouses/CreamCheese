using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Buff card")]

public class Buff_Card : NonGod_Card
{
    public bool isMult;
    public override Card_Behaviour Init(GameObject a)
    {
        Buff_Behaviour behaviour = a.AddComponent<Buff_Behaviour>();
        behaviour.Initialize(this);
        return behaviour;
    }
}
