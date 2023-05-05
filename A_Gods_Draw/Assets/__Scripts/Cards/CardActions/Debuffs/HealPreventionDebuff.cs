using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPreventionDebuff : DebuffBase
{

    public override void UpdateDebuffDisplay()
    {
        
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_HealPrevention_IMG_v1 2"), Stacks, "HealPrevention\nPrevents any healing on this enemy");

    }

    private void Start()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_HealPrevention_IMG_v1 2"), Stacks, "HealPrevention\nPrevents any healing on this enemy");

    }

    public override void RemoveDebuff()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_HealPrevention_IMG_v1 2"), 0, "HealPrevention\nPrevents any healing on this enemy");
        Destroy(this);

    }

    public override void TickDebuff(int _ticks = 1)
    {


        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_HealPrevention_IMG_v1 2"), Stacks, "HealPrevention\nPrevents any healing on this enemy");
        if(Stacks > 0)
            return;

        thisMonster.HealingDisabled = false;
        Destroy(this);

    }

}
