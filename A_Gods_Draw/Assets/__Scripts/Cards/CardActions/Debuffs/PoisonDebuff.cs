using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : DebuffBase
{
    bool wasAppliedThisTurn = true;

    public override void RemoveDebuff()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/poisonDrop"), 0, "Poison\nDeals damage equals to stacks\nper turn");
        Destroy(this);

    }

    private void Start()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/poisonDrop"), Stacks, "Poison\nDeals damage equals to stacks\nper turn");

    }

    public override void TickDebuff(int _ticks = 1)
    {
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/poisonDrop"), Stacks, "Poison\nDeals damage equals to stacks\nper turn");
        
        if(!wasAppliedThisTurn)
        {
            thisMonster.TakeDamage(Stacks, true);
            Stacks -= _ticks;

            if(Stacks <= 0)
                Destroy(this);
        }
        else
            wasAppliedThisTurn = false;        
    }

    public override void PreActTickDebuff(int _ticks = 1)
    {
        thisMonster.UpdateQueuedPoison(Stacks);
    }
}