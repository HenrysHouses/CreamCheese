using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense_Card
{
    public override Card_Behaviour Init(GameObject a) 
    { 
        Defense_Behaviour behaviour = a.AddComponent<Defense_Behaviour>();
        behaviour.Initialize(this);
        return behaviour;
    }
}
