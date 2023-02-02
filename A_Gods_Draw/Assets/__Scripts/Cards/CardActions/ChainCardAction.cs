// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class ChainCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.1f);

        foreach (IMonster monster in cardStats.Targets)
        {
            if (monster)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, monster));
                yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

                ChainedDebuff _chained;
                if(monster.gameObject.TryGetComponent<ChainedDebuff>(out _chained))
                {

                    _chained.Stacks += cardStats.strength;

                }
                else
                {

                    _chained = monster.gameObject.AddComponent<ChainedDebuff>();
                    _chained.Stacks = cardStats.strength;
                    _chained.thisMonster = monster;

                }
                
            }
        }

        cardStats.Targets.Clear();

        isReady = true;
    }

    public override void ResetCamera()
    {
        camAnim.SetBool("EnemyCloseUp", false);
    }
    public override void SetCamera()
    {
        camAnim.SetBool("EnemyCloseUp", true);
    }
}
