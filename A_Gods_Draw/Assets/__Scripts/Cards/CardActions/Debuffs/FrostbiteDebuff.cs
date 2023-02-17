using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteDebuff : DebuffBase
{

    public override void TickDebuff(int _ticks = 1)
    {
        
        Stacks -= _ticks;
        thisMonster.UpdateEffect(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks);

        if(Stacks <= 0)
            Destroy(this);

    }

    public override void PreActDebuff()
    {
        
        thisMonster.DeBuff(Stacks);

    }

    private void Awake()
    {

        PreActDebuff();

    }

}