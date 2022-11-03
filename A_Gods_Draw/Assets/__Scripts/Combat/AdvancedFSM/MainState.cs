using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState : CombatFSMState
{
    TurnController Controller;

    bool HasExitMain = false;

    public MainState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.MainPhase;
    }

    public override void Reason(bool override_ = false)
    {
        // bool shouldTrigger = 
        if(!HasExitMain)
            return;

        Controller.PerformTransition(Transition.EnterDiscard);
        HasExitMain = false;
    }

    public override void Act()
    {
        if (!Controller.shouldEndTurn)
            return;

        HasExitMain = true;
    }
}
