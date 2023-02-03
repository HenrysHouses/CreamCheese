using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrostbiteCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in source.AllTargets)
        {
            
            FrostbiteDebuff _frostbite;
            if(target.gameObject.TryGetComponent<FrostbiteDebuff>(out _frostbite))
            {

                _frostbite.Stacks += source.stats.strength;

            }
            else
            {

                _frostbite = target.gameObject.AddComponent<FrostbiteDebuff>();
                _frostbite.Stacks = source.stats.strength;
                _frostbite.thisMonster = target;

            }

            target.DeBuff(source.stats.strength);

        }
        
        // source.stats.Targets.Clear();

        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }
}
