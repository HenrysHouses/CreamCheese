using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPreventionDebuff : DebuffBase
{

    public override void TickDebuff(int _ticks = 1)
    {

        Stacks -= _ticks;
        if(Stacks > 0)
            return;

        thisMonster.HealingDisabled = false;
        Destroy(this);

    }

}
