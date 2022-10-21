using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAction : Action
{
    public CardAction(int _min, int _max) : base(_min, _max) { }

    public virtual void OnPlay(BoardStateController board) { }
}
 