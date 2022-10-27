using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsState : CombatFSMState
{
    TurnController Controller;

    ushort numOfCardsActed = 0;

    //bool isShowingPanel = false;

    public CardsState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.CombatCardStep;
    }

    public override void Reason(bool override_ = false)
    {
        if ((!Controller.shouldWaitForAnims && numOfCardsActed >= Controller.GetBoard().playedCards.Count/* && !isShowingPanel*/) || Controller.GetBoard().playedCards.Count == 0)
            Controller.PerformTransition(Transition.EnterCombatEnemy);
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
            else if (true/* !isShowingPanel */)
            {
                //Controller.showEnemyTurnPanel() :v
                //isShowingPanel = true;
            }
            else if (true/* isShowingPanel */)
            {
                //isShowingPanel = false;
            }
            Controller.shouldWaitForAnims = true;
        }
    }
}
