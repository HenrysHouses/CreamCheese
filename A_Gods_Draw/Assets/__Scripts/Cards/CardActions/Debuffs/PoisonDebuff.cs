using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : DebuffBase
{
    bool wasAppliedThisTurn = true;

    public override void RemoveDebuff()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Poisoned_IMG_v2"), 0, "Poison\nDeals damage equals to stacks\nper turn");
        Destroy(this);

    }

    private void Start()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Poisoned_IMG_v2"), Stacks, "Poison\nDeals damage equals to stacks\nper turn");

    }

    public override void OnCardsPlayedTickDebuff(int _ticks = 1)
    {

        if(wasAppliedThisTurn)
        {

            wasAppliedThisTurn = false;
            return;

        }

        thisMonster.TakeDamage(Stacks, true);
        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Poisoned_IMG_v2"), Stacks, "Poison\nDeals damage equals to stacks\nper turn");
        if(Stacks <= 0)
            Destroy(this);

    }

    public override void OnDrawActTickDebuff(int _ticks = 1)
    {

        thisMonster.UpdateQueuedPoison(Stacks);

    }

}