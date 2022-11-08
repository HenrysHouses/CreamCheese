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
    PlayerTracker _player;

    public EndState(TurnController controller, PlayerTracker player)
    {
        Controller = controller;
        Controller.isCombatStarted = false;
        stateID = CombatState.EndStep;
        _player = player;

    }

    public override void Reason(bool override_ = false)
    {
        if (Controller.shouldEndTurn)
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

        if (_player.Health <= 0)
        {
            MultiSceneLoader.loadCollection("Death", collectionLoadMode.difference);

            _player.Health = 10;
            return;

        }
        if (Controller.GetBoard().isEnemyDefeated)
            MultiSceneLoader.loadCollection("Map", collectionLoadMode.difference);

    }
}
