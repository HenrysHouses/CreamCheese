// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        yield return new WaitForSeconds(0.2f);
        foreach (Monster target in source.AllTargets)
        {
            if (target)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, target));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.TakeDamage(source.stats.strength);
                yield return new WaitForSeconds(0.1f);
            }
        }

        // source.stats.Targets.Clear();

        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(true, 1, "Action VFX/sword_VFX", "Action VFX/Effect_SlashEffect_VFX", 2);
    }
}
