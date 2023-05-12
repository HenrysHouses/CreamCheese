// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DrawCardAction : CardAction
{
    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    // }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        currentCard.Controller.DrawCardExtra++;
        playSFX(source.gameObject);

        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, null, new Vector3(0, 1, 0)));
        if(_VFX is not null)
            yield return new WaitUntil(() => !_VFX.isAnimating);

        yield return new WaitForSeconds(0.1f);

        isReady = true;
    }

    public override void Reset(BoardStateController board, Card_Behaviour Source)
    {
        ActionCard_Behaviour card = Source as ActionCard_Behaviour;
        // card.stats.Targets.Clear();
        isReady = false;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
    }
}
