using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPreventionDebuff : DebuffBase
{

    private void Start()
    {
        //needs sprite
        //thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/poisonDrop"), Stacks, "HealPrevention\nPrevents any healing on this enemy");

    }

    public override void RemoveDebuff()
    {

        //thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/poisonDrop"), 0, "HealPrevention\nPrevents any healing on this enemy");
        Destroy(this);

    }

    public override void TickDebuff(int _ticks = 1)
    {

        //thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/poisonDrop"), Stacks, "HealPrevention\nPrevents any healing on this enemy");

        Stacks -= _ticks;
        if(Stacks > 0)
            return;

        thisMonster.HealingDisabled = false;
        Destroy(this);

    }

}
