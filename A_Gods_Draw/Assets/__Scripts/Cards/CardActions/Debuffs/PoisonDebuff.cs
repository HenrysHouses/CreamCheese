using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : DebuffBase
{

    public override void TickDebuff(int _ticks = 1)
    {

        thisMonster.DealDamage(Stacks);
        Stacks -= _ticks;

        if(Stacks <= 0)
            Destroy(this);

    }

}