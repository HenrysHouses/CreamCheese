/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : CombatFSMState
{
    TurnController Controller;
    bool hasEndTriggered = false;

    public EndState(TurnController controller)
    {
        Controller = controller;
        Controller.isCombatStarted = false;
        stateID = CombatState.EndStep;
    }

    public override void Reason(bool override_ = false)
    {
        if(Controller.shouldEndTurn)
            return;

        Controller.PerformTransition(Transition.EnterDraw);
        Controller.shouldEndTurn = false;
        hasEndTriggered = false;
    }

    public override void Act()
    {
        BoardElement.ExitCombat();
        // end of combat triggers here
        // hasEndTriggered = true;
        if(Controller.GetBoard().isEnemyDefeated)
        MultiSceneLoader.loadCollection("Map",collectionLoadMode.difference);
    }
}
