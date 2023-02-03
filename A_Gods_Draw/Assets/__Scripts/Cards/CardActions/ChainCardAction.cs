// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class ChainCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.1f);

        foreach (Monster monster in source.AllTargets)
        {
            if (monster)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, monster));
                yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

                ChainedDebuff _chained;
                if(monster.gameObject.TryGetComponent<ChainedDebuff>(out _chained))
                {

                    _chained.Stacks += source.stats.strength;

                }
                else
                {

                    _chained = monster.gameObject.AddComponent<ChainedDebuff>();
                    _chained.Stacks = source.stats.strength;
                    _chained.thisMonster = monster;

                }
                
            }
        }

        // source.stats.Targets.Clear();

        isReady = true;
    }
}
