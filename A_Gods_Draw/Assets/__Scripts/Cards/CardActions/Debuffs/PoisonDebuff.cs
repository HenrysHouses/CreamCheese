using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : DebuffBase
{

    public override void TickDebuff(int _ticks = 1)
    {

        thisMonster.TakeDamage(Stacks, true);
        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks);

        if(Stacks <= 0)
            Destroy(this);

    }

    public override void PreActTickDebuff(int _ticks = 1)
    {
        
        thisMonster.UpdateQueuedPoison(Stacks);

    }

}