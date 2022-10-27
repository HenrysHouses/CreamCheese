using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardAction : CardAction
{
    IMonster target;
    public AttackCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator ChoosingTargets(BoardStateController board, float mult)
    {
        isReady = false;
        //foreach monster in bpard, enable click
        board.SetClickable(3);

        Debug.Log("waiting for selecting enemies...");

        yield return new WaitUntil(HasClickedMonster);

        board.SetClickable(3, false);

        isReady = true;
    }

    bool HasClickedMonster()
    {
        BoardElement element = TurnController.PlayerClick();
        IMonster clickedMonster = element as IMonster;
        if (clickedMonster)
        {
            Debug.Log(clickedMonster);
            target = clickedMonster;
            return true;
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board, int strengh)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        if (target)
            target.DealDamage(strengh);

        isReady = true;
    }
}
