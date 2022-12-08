/* 
 * Written by 
 * Henrik
 * 
 * Modified by Javier
*/

using HH.MultiSceneTools;

/// <summary>
/// A State to handle when the player dies or wins
/// </summary>
public class EndState : CombatFSMState
{
    TurnController Controller;
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
        // hasEndTriggered = false;
        ResetRuneTurnTriggers(Controller);
    }

    public override void Act()
    {
        BoardElement.ExitCombat();
        // end of combat triggers here

        ActivateRune(Controller);

        if (_player.Health <= 0)
        {
            MultiSceneLoader.loadCollection("Death", collectionLoadMode.Difference);
            _player.Health = 10;
            return;
        }
        if (Controller.GetBoard().isEnemyDefeated)
        {
            if (Controller.GetBoard().Encounter.name.Equals("Boss"))
                GameManager.timesDefeatedBoss++;
            MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
        }
    }
}
