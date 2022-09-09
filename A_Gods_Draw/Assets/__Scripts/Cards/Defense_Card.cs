using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Defense card")]

public class Defense_Card : NonGod_Card
{
    public override void Init(GameObject a) 
    { 
        Defense_Behaviour behaviour = a.AddComponent<Defense_Behaviour>();
        behaviour.Initialize(this);
        cardObject = a;
    }
}
