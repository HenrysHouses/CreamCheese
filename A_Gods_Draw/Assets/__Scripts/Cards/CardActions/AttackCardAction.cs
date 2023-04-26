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
                if(target.GetHealth() <= 0)
                    continue;

                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, target));
                playSFX(source.gameObject);
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
        _VFX = new ActionVFX(true, 1, "Action VFX/AxeAttack_VFX", "Action VFX/Effect_SlashEffect_VFX", 2);
    }
}
