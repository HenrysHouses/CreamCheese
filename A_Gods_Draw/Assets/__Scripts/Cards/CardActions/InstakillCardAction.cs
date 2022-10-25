using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillCardAction : CardAction
{
    IMonster target;

    public InstakillCardAction(int strengh) : base(strengh, strengh) { }

    protected override IEnumerator ChoosingTargets(BoardStateController board)
    {

        yield return new WaitUntil(() => true);


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

    protected override IEnumerator OnAction(BoardStateController board)
    {
        //StartAnimations...

        yield return new WaitUntil(() => true);

        if (Random.Range(1, 10) <= max)
            target.DealDamage(10000);

        isReady = true;
    }
}
