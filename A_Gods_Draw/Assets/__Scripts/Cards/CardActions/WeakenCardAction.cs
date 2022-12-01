using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeakenCardAction : CardAction
{
    public WeakenCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in targets)
        {
            if (target)
            {
                target.Weaken(strengh);
            }
        }

        yield return new WaitForSeconds(0.3f);

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
