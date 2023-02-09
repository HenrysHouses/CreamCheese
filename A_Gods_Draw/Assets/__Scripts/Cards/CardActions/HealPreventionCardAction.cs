using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealPreventionCardAction : CardAction
{
    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    // }

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = false;

        foreach (Monster target in _source.AllTargets)
        {
            
            HealPreventionDebuff _healPrev;
            if(target.gameObject.TryGetComponent<HealPreventionDebuff>(out _healPrev))
            {

                _healPrev.Stacks += _source.stats.strength;

            }
            else
            {

                _healPrev = target.gameObject.AddComponent<HealPreventionDebuff>();
                _healPrev.Stacks = _source.stats.strength;
                _healPrev.thisMonster = target;
                target.HealingDisabled = true;

            }

        }
        
        // source.stats.Targets.Clear();

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }
}
