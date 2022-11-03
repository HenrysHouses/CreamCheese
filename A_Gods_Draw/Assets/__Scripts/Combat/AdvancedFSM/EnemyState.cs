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
        if (numOfEnemiesActed >= Controller.GetBoard().Enemies.Length)
        {
            Controller.PerformTransition(Transition.EnterEnd);
            numOfEnemiesActed = 0;
        }
    }

    public override void Act()
    {
        if (/*!Controller.shouldWaitForAnims*/ true)
        {
            if (numOfEnemiesActed < Controller.GetBoard().Enemies.Length)
            {
                Controller.GetBoard().Enemies[numOfEnemiesActed].Act(Controller.GetBoard());
                numOfEnemiesActed++;
            }
            //Controller.shouldWaitForAnims = true;
        }
    }
}
