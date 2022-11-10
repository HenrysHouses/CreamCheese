using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : CombatFSMState
{
    TurnController Controller;

    ushort numOfEnemiesActed = 0;

    public EnemyState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.CombatEnemyStep;
    }

    public override void Reason(bool override_ = false)
    {
        if (numOfEnemiesActed >= Controller.GetBoard().getLivingEnemies().Length)
        {
            Controller.PerformTransition(Transition.EnterEnd);
            numOfEnemiesActed = 0;
        }
    }

    public override void Act()
    {
        if (!TurnController.shouldWaitForAnims)
        {
            if (numOfEnemiesActed < Controller.GetBoard().getLivingEnemies().Length)
            {
                Controller.GetBoard().getLivingEnemies()[numOfEnemiesActed].Act(Controller.GetBoard());
                numOfEnemiesActed++;
            }
        }
    }
}
