using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = false;
        playSFX(_source.gameObject);

        foreach(Monster livingEnemy in _board.getLivingEnemies())
        {
            if(_source.AllTargets.Length == 0) // deal damage to enemies if all targets are dead
            {
                livingEnemy.TakeDamage(_source.stats.strength);
            }
            else // dont deal extra damage to target enemy
            {
                foreach (BoardElement cardTarget in _source.AllTargets)
                {
                    if(livingEnemy.gameObject != cardTarget.gameObject)
                        livingEnemy.TakeDamage(_source.stats.strength);
                }
            }
        }

        // Playing VFX for each action
        if(_source.AllTargets.Length > 0)
            _board.StartCoroutine(playTriggerVFX(_source.gameObject, _source.AllTargets[0] as Monster));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);
        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "Action VFX/Earthquake_VFX", 2);
    }
}
