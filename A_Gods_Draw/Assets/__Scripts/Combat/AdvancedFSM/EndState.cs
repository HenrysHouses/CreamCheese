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
    
    bool WonCombat;

    public EndState(TurnController controller, PlayerTracker player)
    {
        Controller = controller;
        Controller.isCombatStarted = false;
        stateID = CombatState.EndStep;
        _player = player;

    }

    public override void Reason(bool override_ = false)
    {
        if(WonCombat)
            return;

        if (Controller.shouldEndTurn)
            return;

        Controller.PerformTransition(Transition.EnterDraw);
        Controller.shouldEndTurn = false;
        // hasEndTriggered = false;
        ResetRuneTurnTriggers(Controller);

        foreach (Monster _monster in Controller.GetBoard().getLivingEnemies())
        {

            if(!_monster.gameObject.TryGetComponent<DebuffBase>(out DebuffBase _debuffCheck))
                continue;

            foreach (DebuffBase _debuff in _monster.GetComponents<DebuffBase>())
            {

                _debuff.TickDebuff();
                
            }
            
        }

    }

    public override void Act()
    {
        if(WonCombat)
            return;

        BoardElement.ExitCombat(); // ????

        ActivateRune(Controller);

        if (_player.Health <= 0)
        {
           // ??? 
           // MultiSceneLoader.loadCollection("Death", collectionLoadMode.Difference);
        //    DeathCrumbling death;
           
             Controller.PlayerDying();
            _player.Health = 10;
            return;
        }
        if (Controller.GetBoard().isEnemyDefeated)
        {
            UnityEngine.Debug.Log("saved");
            bool defeatedBoss = false;
            if (Controller.GetBoard().Encounter.name.Equals("Boss"))
            {
                GameManager.timesDefeatedBoss++;
                defeatedBoss = true;
            }

            Controller.StartCoroutine(Controller.ExitCombat(defeatedBoss));
            WonCombat = true;
        }
    }
}
