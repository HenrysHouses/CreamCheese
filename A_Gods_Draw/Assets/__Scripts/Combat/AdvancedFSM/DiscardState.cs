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
        if(hasDiscarded && !Controller.isDiscardAnimating)
        {
            Controller.PerformTransition(Transition.EnterEnd);
            hasDiscarded = false;
        }
    }

    public override void Act()
    {
        Controller.DiscardAll();
        hasDiscarded = true;
    }
}
