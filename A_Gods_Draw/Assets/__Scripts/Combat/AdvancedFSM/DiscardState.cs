/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardState : CombatFSMState
{
    TurnController Controller;
    bool hasDiscarded = false;

    public DiscardState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.DiscardStep;
    }

    public override void Reason(bool override_ = false)
    {
        bool shouldTrigger = hasDiscarded 
                        && !Controller.isDiscardAnimating 
                        && !TurnController.shouldWaitForAnims;

        if(!shouldTrigger)
            return;

        Controller.PerformTransition(Transition.EnterCombatStart);
        hasDiscarded = false;
        ResetRuneTurnTriggers(Controller);
    }

    public override void Act()
    {
        if(hasDiscarded)
            return;

        ActivateRune(Controller);
        //god.OnDiscardState();

        Controller.shouldEndTurn = false;
        TurnController.shouldWaitForAnims = true;
        Controller.DiscardAll();
        hasDiscarded = true;
    }
}
