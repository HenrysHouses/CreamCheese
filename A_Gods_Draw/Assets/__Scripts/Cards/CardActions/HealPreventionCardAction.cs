using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealPreventionCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = false;
        playSFX(_source.gameObject);

        foreach (Monster target in _source.AllTargets)
        {
            
            HealPreventionDebuff _healPrev;
            if(target.gameObject.TryGetComponent<HealPreventionDebuff>(out _healPrev))
            {

                _healPrev.Stacks += _source.stats.strength;
                _healPrev.UpdateDebuffDisplay();

            }
            else
            {

                _healPrev = target.gameObject.AddComponent<HealPreventionDebuff>();
                _healPrev.Stacks = _source.stats.strength;
                _healPrev.thisMonster = target;
                target.HealingDisabled = true;

            }

        }
        
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
        Debug.LogError("Heal Prevention has no VFX");
    }
}
