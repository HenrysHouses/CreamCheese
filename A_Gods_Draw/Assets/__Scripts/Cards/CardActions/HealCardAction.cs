using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCardAction : CardAction
{
    public HealCardAction(int strengh) : base(strengh, strengh) { }

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;

        board.Player.Heal(strengh);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        isReady = false;
        ResetCamera();
    }
    public override void ResetCamera()
    {
    }
    public override void SetCamera()
    {
    }
}
