/* 
 * Written by 
 * Henrik
*/

/// <summary>
/// A State to get ready for the combat to start. Triggers Runes that happens at the start of combat
/// </summary>
public class AwakeCombatState : CombatFSMState
{
    TurnController Controller;

    bool hasShuffled = false;

    public AwakeCombatState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.EnterCombat;
    }

    /// <summary>Makes the combat ready to start</summary>
    public override void Reason(bool override_ = false)
    {
        if(!hasShuffled)
            return;

        Controller.PerformTransition(Transition.EnterDraw);
        Controller.isCombatStarted = true;
        hasShuffled = false;
        ResetRuneTurnTriggers(Controller);
        ResetRuneAllRunes(Controller);
    }

    /// <summary>Triggers Runes, Resets rune triggers, Library shuffle</summary>
    public override void Act()
    {
        if(hasShuffled)
            return;

        ActivateRune(Controller);

        Controller.ShuffleLibrary();
        ResetRuneGameTriggers(Controller);
        hasShuffled = true;
    }
}