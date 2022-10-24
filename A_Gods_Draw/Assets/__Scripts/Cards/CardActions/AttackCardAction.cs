using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardAction : CardAction
{
    IMonster target;

    public AttackCardAction(int strengh) : base(strengh, strengh) { }

    protected override IEnumerator ChoosingTargets(BoardStateController board)
    {
        //foreach monster in bpard, enable click
        foreach (IMonster monster in board.Enemies)
        {
            monster.clickable = true;
        }

        yield return new WaitUntil(() => board.GetClickedMonster());
        target = board.GetClickedMonster();
        board.SetClickedMonster();

        foreach (IMonster monster in board.Enemies)
        {
            monster.clickable = false;
        }

        isReady = true;
    }

    protected override IEnumerator OnAction(BoardStateController board)
    {
        //StartAnimations...

        yield return new WaitUntil(() => true);

        target.DealDamage(max);

        isReady = true;
    }
}
