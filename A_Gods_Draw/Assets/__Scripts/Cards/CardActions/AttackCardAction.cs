using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardAction : CardAction
{
    public AttackCardAction(int strengh) : base(strengh, strengh) { }

    public override void Execute(BoardStateController BoardStateController, int strengh)
    {
        throw new System.NotImplementedException();
    }
}
