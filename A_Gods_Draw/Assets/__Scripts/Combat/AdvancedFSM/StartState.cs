using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : CombatFSMState
{
    TurnController Controller;

    public StartState(TurnController controller)
    {
        Controller = controller;
        Controller.isCombatStarted = true;
        stateID = CombatState.CombatStartStep;
    }

    public override void Reason(bool override_ = false)
    {
        Controller.PerformTransition(Transition.EnterCombatCard); // ! this should be enter main not discard
    }

    public override void Act()
    {
        //god.OnStartState()
    }
}

