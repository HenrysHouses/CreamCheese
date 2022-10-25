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
        board.SetClickable(1);
        board.SetClickable(2);

        yield return new WaitUntil(HasClickedGodOrPlayer);

        board.SetClickable(1, false);
        board.SetClickable(2, false);

        isReady = true;
    }

    bool HasClickedGodOrPlayer()
    {
        BoardElement element = TurnController.PlayerClick();
        God_Behaviour clickedCard = element as God_Behaviour;
        PlayerController clickedplayer = element as PlayerController;
        if (clickedCard)
        {
            godTarget = clickedCard;
            return true;
        }
        else if (clickedplayer)
        {
            playerTarget = clickedplayer;
            return true;
        }
        return false;
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
