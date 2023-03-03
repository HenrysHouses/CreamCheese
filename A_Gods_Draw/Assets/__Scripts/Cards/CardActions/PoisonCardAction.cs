using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoisonCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = false;

        foreach (Monster target in _source.AllTargets)
        {

            if(!target)
                continue;
            
            PoisonDebuff _poison;
            if(target.gameObject.TryGetComponent<PoisonDebuff>(out _poison))
            {

                _poison.Stacks += _source.stats.strength;

            }
            else
            {

                _poison = target.gameObject.AddComponent<PoisonDebuff>();
                _poison.Stacks = _source.stats.strength;
                _poison.thisMonster = target;

            }

            _board.StartCoroutine(playTriggerVFX(_source.gameObject, target.transform, new Vector3(0, 0, 0)));
        }
        
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
        
    }
}
