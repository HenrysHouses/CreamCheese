using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeachCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, NonGod_Behaviour _source)
    {

        isReady = false;

        foreach(IMonster _target in cardStats.Targets)
        {

            int _damageDealt = _target.DealDamage(cardStats.strength);
            _board.Player.Heal(_damageDealt);

        }

        cardStats.Targets.Clear();

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;

    }

    public override void ResetCamera()
    {}

    public override void SetCamera()
    {}



}
