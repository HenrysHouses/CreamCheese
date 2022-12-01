using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardAction : CardAction
{
    public AttackCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        yield return new WaitForSeconds(0.6f);
        foreach (IMonster target in targets)
        {
            if (target)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, target));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.DealDamage(strengh);
                yield return new WaitForSeconds(0.2f);
            }
        }

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
