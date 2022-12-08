/* 
 * Written by 
 * Henrik
 * 
 * Modified by Javier
*/

/// <summary>
/// A State to enter combat
/// </summary>
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
        BoardElement.EnterCombat();
        TurnController.shouldWaitForAnims = false;
        Controller.PerformTransition(Transition.EnterCombatCard); 
        ResetRuneTurnTriggers(Controller);
    }

    public override void Act()
    {
        ActivateRune(Controller);
    }
}

