using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeachCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = false;
        playSFX(_source.gameObject);

        foreach (Monster target in _source.AllTargets)
        {
            
            LeachDebuff _leach;
            if(target.gameObject.TryGetComponent<LeachDebuff>(out _leach))
            {

                _leach.Stacks += _source.stats.strength;
                _leach.UpdateDebuffDisplay();

            }
            else
            {

                _leach = target.gameObject.AddComponent<LeachDebuff>();
                _leach.Stacks = _source.stats.strength;
                _leach.thisMonster = target;
                target.Leached = true;

            }

        }
        
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(true, 1, "", "Action VFX/LeechEffect_VFX", 2);
    }
}
