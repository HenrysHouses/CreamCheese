using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonGod_Behaviour : Card_Behaviour
{
    public virtual void GetGodBuff(bool isMultiplier, float amount) { }
    public virtual void GetBuff(bool isMultiplier, short amount) { }
}
