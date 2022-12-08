// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class DrawCardAction : CardAction
{
    public DrawCardAction(int strengh) : base(strengh, strengh) { }

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
    }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        current.Controller.DrawCardExtra = strengh;

        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, null, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => !_VFX.isAnimating);

        yield return new WaitForSeconds(0.1f);

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

    public override void Buff(int amount, bool isMult)
    {
    }
    public override void DeBuff(int amount, bool isMult)
    {
    }
}
