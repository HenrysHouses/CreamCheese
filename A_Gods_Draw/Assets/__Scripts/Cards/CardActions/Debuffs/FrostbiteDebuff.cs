using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteDebuff : DebuffBase
{

    public override void PreActTickDebuff(int _ticks = 1)
    {

        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"), Stacks, "Frostbite\nWeakens the enemy's actions ");

        if(Stacks <= 0)
        {

            Destroy(this);
            return;

        }
        
        thisMonster.DeBuff(Stacks);

    }

    private void Start()
    {

        PreActTickDebuff();

    }
}