using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Attack card")]

public class Attack_Card : NonGod_Card
{
    public override void Init(GameObject a)
    {
        Attack_Behaviour behaviour = a.AddComponent<Attack_Behaviour>();
        behaviour.Initialize(this);
        cardObject = a;
    }
}
