using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCardAction : CardAction
{
    IMonster target;

    public DefendCardAction(int strengh) : base(strengh, strengh) { }


    public override IEnumerator ChoosingTargets(BoardStateController board)
    {
        isReady = false;

        board.SetClickable(3);

        yield return new WaitUntil(HasClickedMonster);

        board.SetClickable(3, false);

        // target.reduceDamage() ?

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

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;
        yield return new WaitUntil(() => true);

        isReady = true;
    }
}
