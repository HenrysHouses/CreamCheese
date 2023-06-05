using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedDebuff : DebuffBase
{

    public override void UpdateDebuffDisplay()
    {
        
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks, "Chains\nPrevents enemies to take action");

    }

    private void Start()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks, "Chains\nPrevents enemies to take action");
        thisMonster.CancelIntent();
        
    }

    public override void RemoveDebuff()
    {
        
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), 0, "Chains\nPrevents enemies to take action");
        Destroy(this);

    }

    public override void OnCardsPlayedTickDebuff(int _ticks = 1)
    {
        
        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks, "Chains\nPrevents enemies to take action");

        if(Stacks <= 0)
        {

            Destroy(this);
            return;
    
        }
        
    }

    public override void OnDrawActTickDebuff(int _ticks = 1)
    {
        
        thisMonster.CancelIntent();

    }

}
