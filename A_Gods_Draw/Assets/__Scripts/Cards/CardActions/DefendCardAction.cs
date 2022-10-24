using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCardAction : CardAction
{
    God_Behaviour godTarget;
    PlayerController playerTarget;

    public DefendCardAction(int strengh) : base(strengh, strengh) { }

    protected override IEnumerator ChoosingTargets(BoardStateController board)
    {
        playerTarget.clickable = true;
        godTarget.clickable = true;

        yield return new WaitUntil(() => board.GetClickedGod() || board.GetClickedPlayer());
        godTarget = board.GetClickedGod();
        playerTarget = board.GetClickedPlayer();
        board.SetClickedGod();
        board.SetClickedPlayer();

        playerTarget.clickable = false;
        godTarget.clickable = false;

        isReady = true;
    }

    protected override IEnumerator OnAction(BoardStateController board)
    {
        yield return new WaitUntil(() => true);

        if (godTarget)
        {
            godTarget.Defend(max);
        }
        else if (playerTarget)
        {
            playerTarget.Defend(max);
        }

        isReady = true;
    }
}
