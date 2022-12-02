using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainCardAction : CardAction
{

    public ChainCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        foreach (IMonster monster in targets)
        {
            if (monster)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, monster));
                yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);
                monster.GetIntent().CancelIntent();
                monster.SetOverlay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"));
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
