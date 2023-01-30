using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBase : MonoBehaviour
{

    public int Stacks;
    public IMonster thisMonster;

    ///<Summary> Use this to have debuff do its thing after the turn and tick the debuff adjusting it's stacks </Summary>
    public virtual void TickDebuff(int _ticks = 1){}

    ///<Summary> Use this to apply debuffs that should happen before end of turn (Happens before playing cards) </Summary>
    public virtual void PreActDebuff(){}

}
