// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class InstakillCardAction : CardAction
{
    IMonster target;

    public InstakillCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;


        if (Random.Range(1, 10) <= strength)
        {
            // Playing VFX
            board.StartCoroutine(playTriggerVFX(target.gameObject, null, new Vector3(0, 1 ,0)));
            yield return new WaitUntil(() => !_VFX.isAnimating);
            target.DealDamage(10000);
        }

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        target = null;
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
