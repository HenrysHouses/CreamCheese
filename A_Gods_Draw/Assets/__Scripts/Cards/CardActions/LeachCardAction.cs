using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeachCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {

        isReady = false;

        foreach(IMonster _target in _source.AllTargets)
        {

            int _damageDealt = _target.DealDamage(_source.stats.strength);
            _board.Player.Heal(_damageDealt);

        }

        // _source.stats.Targets.Clear();

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;

    }
}