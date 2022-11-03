using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsState : CombatFSMState
{
    TurnController Controller;

    ushort numOfCardsActed = 0;
    private bool readyToMoveOn = false;

    //bool isShowingPanel = false;

    public CardsState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.CombatCardStep;
    }

    public override void Reason(bool override_ = false)
    {
        if (readyToMoveOn)
        {
            Controller.PerformTransition(Transition.EnterCombatEnemy);
            numOfCardsActed = 0;
            readyToMoveOn = false;
        }
    }

    public override void Act()
    {
        if (!Controller.shouldWaitForAnims)
        {
            if (numOfCardsActed < Controller.GetBoard().playedCards.Count)
            {
                Controller.GetBoard().playedCards[numOfCardsActed].OnAction();
                numOfCardsActed++;
            }
            else
            {
                Controller.GetBoard().playedCards.Clear();
                readyToMoveOn = true;
            }
            Controller.shouldWaitForAnims = true;
        }
    }
}
