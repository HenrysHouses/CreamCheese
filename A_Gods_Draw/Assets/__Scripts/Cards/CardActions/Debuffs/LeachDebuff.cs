using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeachDebuff : DebuffBase
{

    public override void UpdateDebuffDisplay()
    {
        
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Leech_Sprite_v1 1"), Stacks, "Leach\nHeals the player everytime you damage this enemy");

    }

    private void Start()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Leech_Sprite_v1 1"), Stacks, "Leach\nHeals the player everytime you damage this enemy");

    }

    public override void RemoveDebuff()
    {

        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Leech_Sprite_v1 1"), 0, "Leach\nHeals the player everytime you damage this enemy");
        Destroy(this);

    }

    public override void TickDebuff(int _ticks = 1)
    {

        Stacks -= _ticks;
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Leech_Sprite_v1 1"), Stacks, "Leach\nHeals the player everytime you damage this enemy");
        if(Stacks > 0)
            return;

        thisMonster.Leached = false;
        Destroy(this);

    }

}