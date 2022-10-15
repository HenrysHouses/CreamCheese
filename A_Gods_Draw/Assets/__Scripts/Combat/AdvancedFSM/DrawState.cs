using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawState : CombatFSMState
{
    TurnController Controller;
    bool hasDrawn = false;

    public DrawState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.DrawStep;
    }

    public override void Reason(bool override_ = false)
    {
        if(hasDrawn && !Controller.isDrawAnimating && !Controller.shouldWaitForAnims)
        {
            Debug.Log(Controller.shouldWaitForAnims);
            Controller.PerformTransition(Transition.EnterDiscard); // ! this should be enter main not discard
            Debug.Log("temporary skip to discard"); 
            hasDrawn = false;
        }
    }

    public override void Act()
    {
        if(!hasDrawn)
        {
            Controller.Draw(Controller.DrawStepCardAmount);
            hasDrawn = true;
            Controller.shouldWaitForAnims = true;
        }
    }
}