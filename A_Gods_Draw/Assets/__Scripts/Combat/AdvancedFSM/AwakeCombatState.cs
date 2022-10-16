/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeCombatState : CombatFSMState
{
    TurnController Controller;

    bool hasShuffled = false;

    public AwakeCombatState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.EnterCombat;
    }

    public override void Reason(bool override_ = false)
    {
        if(!hasShuffled)
            return;

        Controller.PerformTransition(Transition.EnterDraw);
        Controller.isCombatStarted = true;
        hasShuffled = false;
    }

    public override void Act()
    {
        if(hasShuffled)
            return;

        Controller.ShuffleLibrary();
        hasShuffled = true;
    }
}