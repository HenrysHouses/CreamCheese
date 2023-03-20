using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedDebuff : DebuffBase
{

    private void Start()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks, "Chains\nPrevents enemy to take action");
        thisMonster.CancelIntent();

    }

    public override void PreActTickDebuff(int _ticks = 1)
    {
        
        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks, "Chains\nPrevents enemy to take action");

        if(Stacks <= 0)
        {

            Destroy(this);
            return;
    
        }

        thisMonster.CancelIntent();
        
    }

}
