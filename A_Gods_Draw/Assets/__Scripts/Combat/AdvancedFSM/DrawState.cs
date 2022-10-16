/* 
 * Written by 
 * Henrik
*/

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
        bool shouldTrigger = hasDrawn 
                        && !Controller.isDrawAnimating 
                        && !Controller.shouldWaitForAnims; 
        
        if(!shouldTrigger)
            return;

        Controller.PerformTransition(Transition.EnterMain);
        hasDrawn = false;
    }

    public override void Act()
    {
        if(hasDrawn)
            return;

        Controller.Draw(Controller.DrawStepCardAmount);
        hasDrawn = true;
        Controller.shouldWaitForAnims = true;
    }
}