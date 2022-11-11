using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCardAction : CardAction
{

    public DefendCardAction(int strengh) : base(strengh, strengh) { }


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

        foreach (IMonster target in targets)
        {
            if (target)
                target.DeBuff(strengh);
        }
        targets.Clear();

        yield return new WaitForSeconds(0.4f);

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
