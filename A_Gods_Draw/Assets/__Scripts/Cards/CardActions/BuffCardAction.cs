using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCardAction : CardAction
{
    bool multiplies;
    NonGod_Behaviour target;

    public BuffCardAction(int strengh, bool mult) : base(strengh, strengh) { multiplies = mult; }

    protected override IEnumerator ChoosingTargets(BoardStateController board)
    {
        //foreach monster in bpard, enable click
        foreach (NonGod_Behaviour card in board.playedCards)
        {
            card.clickable = true;
        }

        yield return new WaitUntil(() => board.GetClickedCard());
        target = board.GetClickedCard();
        board.SetClickedCard();

        foreach (NonGod_Behaviour card in board.playedCards)
        {
            card.clickable = true;
        }

        isReady = true;
    }

    protected override IEnumerator OnAction(BoardStateController board)
    {
        //StartAnimations...

        yield return new WaitUntil(() => true);

        target.Buff(max, multiplies);

        isReady = true;
    }
}
