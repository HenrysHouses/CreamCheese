// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class HealCardAction : CardAction
{
    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
    }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        board.Player.Heal(cardStats.strength);
        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

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
