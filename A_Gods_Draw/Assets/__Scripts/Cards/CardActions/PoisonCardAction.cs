using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCardAction : CardAction
{
    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    // }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in source.stats.Targets)
        {
            
            PoisonDebuff _poison;
            if(target.gameObject.TryGetComponent<PoisonDebuff>(out _poison))
            {

                _poison.Stacks += source.stats.strength;

            }
            else
            {

                _poison = target.gameObject.AddComponent<PoisonDebuff>();
                _poison.Stacks = source.stats.strength;
                _poison.thisMonster = target;

            }

        }
        
        source.stats.Targets.Clear();

        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }
}
