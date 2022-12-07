/* 
 * Written by 
 * Henrik
*/

/// <summary>
/// A State to wait for the player to make their choices and play cards.
/// </summary>
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
        ResetRuneTurnTriggers(Controller);
    }

    public override void Act()
    {
        if (!Controller.shouldEndTurn)
            return;

        ActivateRune(Controller);
        HasExitMain = true;
    }
}
