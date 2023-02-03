// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class WeakenCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in source.stats.Targets)
        {
            if (target)
            {
                // Playing VFX
                board.StartCoroutine(playTriggerVFX(target.gameObject, null, new Vector3(0, 1 ,0)));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.Weaken(source.stats.strength);
            }
        }

        yield return new WaitForSeconds(0.3f);

        source.stats.Targets.Clear();

        isReady = true;
    }
}
