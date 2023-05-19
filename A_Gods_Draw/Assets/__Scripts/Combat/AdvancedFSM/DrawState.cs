/* 
 * Written by 
 * Henrik
 * 
 * Modified by Javier
*/

/// <summary>
/// A State to draw draw cards, trigger their runes and update the enemies intents.
/// </summary>
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
                        && !TurnController.shouldWaitForAnims
                        && !Controller.isDrawing
                        && !Controller.isShuffling;

        if (!shouldTrigger)
            return;

        Controller.PerformTransition(Transition.EnterMain);
        hasDrawn = false;
        ResetRuneTurnTriggers(Controller);
    }

    public override void Act()
    {
        if(hasDrawn )
        {
            return;
        }
        Controller.Draw(Controller.DrawStepCardAmount + Controller.DrawCardExtra);
        
        ActivateRune(Controller);
        Controller.DrawCardExtra = 0;

        foreach (Monster monster in Controller.GetBoard().getLivingEnemies())
        {
            monster.DecideIntent(Controller.GetBoard());
        }
        foreach (Monster monster in Controller.GetBoard().getLivingEnemies())
        {
            monster.LateDecideIntent(Controller.GetBoard());

            if(!monster.gameObject.TryGetComponent<DebuffBase>(out DebuffBase _debuffCheck))
                continue;

            foreach (DebuffBase _debuff in monster.GetComponents<DebuffBase>())
            {

                _debuff.OnDrawActTickDebuff();
                
            }
        }

        hasDrawn = true;

        TurnController.shouldWaitForAnims = true;
    }
}