using UnityEngine;

/// <summary>
/// A State to trigger the player's played cards and runes.
/// </summary>
public class CardsState : CombatFSMState
{
    TurnController Controller;

    ushort numOfCardsActed = 0;
    private bool readyToMoveOn = false;


    public CardsState(TurnController controller)
    {
        Controller = controller;
        stateID = CombatState.CombatCardStep;
    }

    public override void Reason(bool override_ = false)
    {
        if (readyToMoveOn)
        {
            Controller.PerformTransition(Transition.EnterCombatEnemy);
            numOfCardsActed = 0;
            readyToMoveOn = false;
            TurnController.shouldWaitForAnims = false;
            ResetRuneTurnTriggers(Controller);
        }
    }

    /// <summary>Triggers the player's runes, then cards in order</summary>
    public override void Act()
    {
        if (!TurnController.shouldWaitForAnims)
        {
            ActivateRune(Controller);

            if (numOfCardsActed < Controller.GetBoard().allPlayedCards.Count)
            {
                Controller.GetBoard().allPlayedCards[numOfCardsActed].OnAction();
                numOfCardsActed++;
            }
            else
            {
                Controller.GetBoard().allPlayedCards.Clear();
                Controller.GetBoard().placedCards.Clear();
                foreach (BoardElement thing in Controller.GetBoard().thingsInLane)
                {
                    if (thing)
                        Object.Destroy(thing.gameObject);
                }
                foreach (Monster monster in Controller.GetBoard().getLivingEnemies())
                {

                    if(!monster.gameObject.TryGetComponent<DebuffBase>(out DebuffBase _debuffCheck))
                        continue;

                    foreach (DebuffBase _debuff in monster.GetComponents<DebuffBase>())
                    {

                        _debuff.OnCardsPlayedTickDebuff();
                        
                    }

                }
                Controller.GetBoard().thingsInLane.Clear();
                readyToMoveOn = true;
            }
            TurnController.shouldWaitForAnims = true;
        }
    }
}
