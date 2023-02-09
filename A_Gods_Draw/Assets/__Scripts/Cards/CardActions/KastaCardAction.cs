using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KastaCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {

        isReady = false;

        foreach(BoardElement _element in _source.AllTargets)
        {

            _board.RemoveFromLane(_element);

            Monster[] _enemies = _board.getLivingEnemies();

            _enemies[0].DealDamage(_source.stats.strength);

        }

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;

    }

}
