using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillCardAction : CardAction
{
    IMonster target;

    public InstakillCardAction(int strengh) : base(strengh, strengh) { }


    public override IEnumerator ChoosingTargets(BoardStateController board, float mult)
    {

        camAnim.SetBool("EnemyCloseUp", true);
        isReady = false;
        //foreach monster in bpard, enable click
        board.SetClickable(3);

        Debug.Log("waiting for selecting enemies...");

        yield return new WaitUntil(HasClickedMonster);

        camAnim.SetBool("EnemyCloseUp", false);

        board.SetClickable(3, false);

        isReady = true;
    }

    bool HasClickedMonster()
    {
        BoardElement element = TurnController.PlayerClick();
        IMonster clickedMonster = element as IMonster;
        if (clickedMonster)
        {
            target = clickedMonster;
            return true;
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board, int strengh)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        if (Random.Range(1, 10) <= strengh)
            target.DealDamage(10000);

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
}
