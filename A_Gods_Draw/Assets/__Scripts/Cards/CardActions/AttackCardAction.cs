using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardAction : CardAction
{
    public AttackCardAction(int strengh) : base(strengh, strengh) { }

    //bool HasClickedMonster()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        BoardElement element = TurnController.PlayerClick();
    //        IMonster clickedMonster = element as IMonster;
    //        if (clickedMonster)
    //        {
    //            Debug.Log(clickedMonster);
    //            target = clickedMonster;
    //            return true;
    //        }
    //        current.MissClick();
    //    }
    //    return false;
    //}

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;

        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        foreach (IMonster target in targets)
        {
            if (target)
                target.DealDamage(strengh);
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
