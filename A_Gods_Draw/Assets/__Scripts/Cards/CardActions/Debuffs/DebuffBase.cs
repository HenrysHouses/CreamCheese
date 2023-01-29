using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBase : MonoBehaviour
{

    public int Stacks;
    public IMonster thisMonster;

    ///<Summary> Use this to have debuff do its thing</Summary>
    public virtual void TickDebuff(int _ticks = 1){}

}
