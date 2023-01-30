// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class ChainCardAction : CardAction
{

    public ChainCardAction(int strength) : base(strength, strength) { }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.1f);

        foreach (IMonster monster in targets)
        {
            if (monster)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, monster));
                yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

                ChainedDebuff _chained;
                if(monster.gameObject.TryGetComponent<ChainedDebuff>(out _chained))
                {

                    _chained.Stacks += strength;

                }
                else
                {

                    _chained = monster.gameObject.AddComponent<ChainedDebuff>();
                    _chained.Stacks = strength;
                    _chained.thisMonster = monster;

                }
                
            }
        }

        targets.Clear();

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        targets.Clear();
        isReady = false;
        board.SetClickable(3, false);
        ResetCamera();
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
