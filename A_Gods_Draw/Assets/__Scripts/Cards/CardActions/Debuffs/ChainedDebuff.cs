using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedDebuff : DebuffBase
{

    public override void TickDebuff(int _ticks = 1)
    {

        thisMonster.GetIntent().CancelIntent();
        thisMonster.ShowEffect(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"));
        Stacks -= _ticks;

        if(Stacks <= 0)
            Destroy(this);

    }

}
