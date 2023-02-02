// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class AttackCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        yield return new WaitForSeconds(0.2f);
        foreach (IMonster target in cardStats.Targets)
        {
            if (target)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, target));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.DealDamage(cardStats.strength);
                yield return new WaitForSeconds(0.1f);
            }
        }

        cardStats.Targets.Clear();

        isReady = true;
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
