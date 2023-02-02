// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class WeakenCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in targets)
        {
            if (target)
            {
                // Playing VFX
                board.StartCoroutine(playTriggerVFX(target.gameObject, null, new Vector3(0, 1 ,0)));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.Weaken(cardStats.strength);
            }
        }

        yield return new WaitForSeconds(0.3f);

        targets.Clear();

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        targets.Clear();
        isReady = false;
        board.SetClickable(3, false);
        ResetCamera();
    }
    public override void ResetCamera()
    {
        camAnim.SetBool("EnemyCloseUp", false);
    }
    public override void SetCamera()
    {
        camAnim.SetBool("EnemyCloseUp", true);
    }


}
