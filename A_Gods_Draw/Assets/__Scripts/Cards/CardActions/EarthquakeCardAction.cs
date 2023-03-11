using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {

        isReady = false;

        foreach(Monster _target in _board.getLivingEnemies())
        {

            _target.TakeDamage(_source.stats.strength);

        }

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _source.AllTargets[0] as Monster));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "Action VFX/Splash_VFX", 2);
    }

}
