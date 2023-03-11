using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = true;
        yield break;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
    }
}
