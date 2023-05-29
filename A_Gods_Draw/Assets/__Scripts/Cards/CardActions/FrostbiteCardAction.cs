using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrostbiteCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = false;
        playSFX(_source.gameObject);

        foreach (Monster target in _source.AllTargets)
        {

            if(target == null)
                yield break;
            
            FrostbiteDebuff _frostbite;
            if(target.gameObject.TryGetComponent<FrostbiteDebuff>(out _frostbite))
            {

                _frostbite.Stacks += _source.stats.strength;
                _frostbite.UpdateDebuffDisplay();

            }
            else
            {

                _frostbite = target.gameObject.AddComponent<FrostbiteDebuff>();
                _frostbite.Stacks = _source.stats.strength;
                _frostbite.thisMonster = target;

            }

        }
        
        // source.stats.Targets.Clear();

        // Playing VFX for each action
        _board.StartCoroutine(playTriggerVFX(_source.gameObject, _board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "Action VFX/Frostbite_VFX", 5);
    }
}
