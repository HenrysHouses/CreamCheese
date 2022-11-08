using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCardAction : CardAction
{

    public DefendCardAction(int strengh) : base(strengh, strengh) { }


    public override IEnumerator ChoosingTargets(BoardStateController board, float mult)
    {
        camAnim.SetBool("EnemyCloseUp", true);
        isReady = false;

        board.SetClickable(3);

        yield return new WaitUntil(HasClickedMonster);

         camAnim.SetBool("EnemyCloseUp", false);

        board.SetClickable(3, false);


        isReady = true;
    }

    bool HasClickedMonster()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoardElement element = TurnController.PlayerClick();
            IMonster clickedMonster = element as IMonster;
            if (clickedMonster)
            {
                Debug.Log(clickedMonster);
                target = clickedMonster;
                return true;
            }
            current.MissClick();
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;

        if (target)
            target.DeBuff(strengh);

        yield return new WaitForSeconds(0.4f);

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
