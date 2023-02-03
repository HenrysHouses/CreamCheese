// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class DrawCardAction : CardAction
{
    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    // }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        currentCard.Controller.DrawCardExtra = source.stats.strength;

        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, null, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => !_VFX.isAnimating);

        yield return new WaitForSeconds(0.1f);

        isReady = true;
    }

    public override void Reset(BoardStateController board, Card_Behaviour Source)
    {
        NonGod_Behaviour card = Source as NonGod_Behaviour;
        card.stats.Targets.Clear();
        isReady = false;
    }
}
