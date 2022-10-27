using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCardAction : CardAction
{
    bool multiplies;
    NonGod_Behaviour target;

    public BuffCardAction(int strengh, bool mult) : base(strengh, strengh) { multiplies = mult; }

    public override IEnumerator ChoosingTargets(BoardStateController board)
    {
        isReady = false;

        board.SetClickable(0);

        yield return new WaitUntil(HasClickedNonGod);

        board.SetClickable(0, false);

        isReady = true;
    }

    bool HasClickedNonGod()
    {
        BoardElement element = TurnController.PlayerClick();
        NonGod_Behaviour clickedCard = element as NonGod_Behaviour;
        if (clickedCard)
        {
            Debug.Log("To buff: " + clickedCard);
            target = clickedCard;
            return true;
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        target.Buff(max, multiplies);

        isReady = true;
    }
}
