using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCardAction : CardAction
{

    //Use the drawstate from fsm to know when new turn happens to tick poison.

    public PoisonCardAction(int strengh) : base(strengh, strengh) { }

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
    }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster monster in board.getLivingEnemies())
        {
            
        }
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
