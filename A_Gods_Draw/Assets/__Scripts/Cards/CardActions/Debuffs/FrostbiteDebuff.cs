using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteDebuff : DebuffBase
{

    public override void TickDebuff(int _ticks = 1)
    {
        
        Stacks -= _ticks;
        if(Stacks <= 0)
            Destroy(this);

    }

    public override void PreActDebuff()
    {
        
        thisMonster.DeBuff(Stacks);

    }

}