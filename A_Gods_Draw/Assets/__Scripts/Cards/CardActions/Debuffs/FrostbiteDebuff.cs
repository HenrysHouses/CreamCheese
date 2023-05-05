using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteDebuff : DebuffBase
{

    public override void UpdateDebuffDisplay()
    {
        
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Frostbite_IMG_v1 1"), Stacks, "Frostbite\nWeakens the enemy's actions ");

    }

    public override void RemoveDebuff()
    {
        
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Frostbite_IMG_v1 1"), 0, "Frostbite\nWeakens the enemy's actions ");
        Destroy(this);

    }

    public override void OnDrawActTickDebuff(int _ticks = 1)
    {

        thisMonster.Weaken(Stacks);
        thisMonster.UpdateEffectDisplay(Resources.Load<Sprite>("EnemyData/Icons/Glyph_Frostbite_IMG_v1 1"), Stacks, "Frostbite\nWeakens the enemy's actions ");
        Stacks -= _ticks;
        
    }

    public override void TickDebuff(int _ticks = 1)
    {

        if(Stacks <= 0)
        {

            Destroy(this);
            return;

        }
        
    }

    private void Start()
    {

        OnDrawActTickDebuff();

    }
}