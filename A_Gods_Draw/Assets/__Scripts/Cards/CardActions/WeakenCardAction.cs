// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class WeakenCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        foreach (Monster target in source.AllTargets)
        {
            if (target && _VFX != null)
            {
                // Playing VFX
                board.StartCoroutine(playTriggerVFX(target.gameObject, null, new Vector3(0, 1 ,0)));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                //target.Weaken(source.stats.strength);
            }
        }

        yield return new WaitForSeconds(0.3f);

        // source.stats.Targets.Clear();

        isReady = true;
    }
}
