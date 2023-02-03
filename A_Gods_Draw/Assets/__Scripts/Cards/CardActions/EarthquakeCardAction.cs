using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {

        isReady = false;

        foreach(IMonster _target in _board.getLivingEnemies())
        {

            _target.DealDamage(currentCard.stats.strength);

        }

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;

    }

}