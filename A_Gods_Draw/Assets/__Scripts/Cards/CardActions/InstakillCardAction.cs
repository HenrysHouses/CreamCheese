using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillCardAction : CardAction
{
    IMonster target;

    public InstakillCardAction(int strengh) : base(strengh, strengh) { }

    protected override IEnumerator ChoosingTargets(BoardStateController board)
    {
        //foreach monster in bpard, enable click
        foreach (IMonster monster in board.Enemies)
        {
            //Enable monster clickable
        }

        yield return new WaitUntil(() => board.GetClickedMonster());
        target = board.GetClickedMonster();
        board.SetClickedMonster();

        foreach (IMonster monster in board.Enemies)
        {
            //Disable monster clickable
        }

        isReady = true;
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
