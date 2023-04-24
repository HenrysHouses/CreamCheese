// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DefendCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        foreach (BoardElement target in source.AllTargets)
        {
            Monster Enemy = target as Monster;

            if (Enemy)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, Enemy.transform, new Vector3(0,0,0)));
                playSFX(source.gameObject);
                if(_VFX != null)
                    yield return new WaitUntil(() => !_VFX.isAnimating);
                Enemy.Weaken(source.stats.strength, true);
            }
        }
        // source.stats.Targets.Clear();
        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 2, "Action VFX/Shield_VFX", "", 0);
    }
}
