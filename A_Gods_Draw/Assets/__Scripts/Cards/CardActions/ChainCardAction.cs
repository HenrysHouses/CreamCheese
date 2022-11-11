using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCardAction : CardAction
{
    public new List<IMonster> targets = new();
    private int totalTargets = 1;

    public ChainCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        foreach (IMonster monster in targets)
        {
            if (monster)
            {
                monster.GetIntent().CancelIntent();
                monster.UpdateUI();
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
