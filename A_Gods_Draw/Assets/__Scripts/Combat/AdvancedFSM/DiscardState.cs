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
                        && !Controller.shouldWaitForAnims;

        if(!shouldTrigger)
            return;

        Controller.PerformTransition(Transition.EnterEnd);
        hasDiscarded = false;
    }

    public override void Act()
    {
        if(hasDiscarded)
            return;

        Controller.shouldEndTurn = false;
        Controller.shouldWaitForAnims = true;
        Controller.DiscardAll(0.25f);
        hasDiscarded = true;
    }
}
