using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorial : TutorialController
{
    private void Awake() {
        GameManager.instance.nextCombatType = EncounterDifficulty.Tutorial;

        for (int i = 0; i < EnableBoard.Length; i++)
        {
            EnableBoard[i].SetActive(false);
        }
        Horn.CanEndTurn = false;
    }

    public override void CheckTutorialConditions()
    {
        if(isTutorialStep(0))
            startTutorialRoutine(PressAnyButtonToContinue(Step1_Trigger));
        
        if(isTutorialStep(1))
            startTutorialRoutine(PlayAttackCardOnEnemy());
    
        if(isTutorialStep(2))
            startTutorialRoutine(EndTurn());

        if(isTutorialStep(3))
            startTutorialRoutine(AttackAgain());

        if(isTutorialStep(4))
            startTutorialRoutine(DefendYourself());
    }

    public GameObject[] EnableBoard;
    public TutorialStepTrigger Step1_Trigger;

    protected IEnumerator PressAnyButtonToContinue(TutorialStepTrigger trigger)
    {
        bool isAllowedToContinue = false;
        bool previousState = CurrentDialogue.fullPageIsDisplaying;

        while(isAllowedToContinue == false)
        {
            yield return new WaitForEndOfFrame();

            if(CurrentDialogue.fullPageIsDisplaying == previousState && CurrentDialogue.fullPageIsDisplaying)
            {
                isAllowedToContinue = true;
            }
            previousState = CurrentDialogue.fullPageIsDisplaying;
        }

        yield return new WaitUntil(() => Input.anyKeyDown); 

        for (int i = 0; i < EnableBoard.Length; i++)
        {
            EnableBoard[i].SetActive(true);
        }
        completeTutorialRoutine(trigger, 0);
    }

    public TurnController turnController;
    public TutorialStepTrigger PlayAttack_Trigger;

    IEnumerator PlayAttackCardOnEnemy()
    {
        monster = turnController.GetBoard().getLivingEnemies()[0] as TutorialMonster;
        yield return new WaitUntil(() => monster != null);
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.Defend);
        Debug.Log("yesees");
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        Debug.Log("main phase");
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Debug.Log("end your turn yess");
        Horn.turnEnd.AddListener(HasEndedTurn);
        Horn.CanEndTurn = true;
        deckController.clear();
        completeTutorialRoutine(PlayAttack_Trigger, 1);
    }

    public EndTurnButton Horn;
    public TutorialStepTrigger EndTurn1st_Trigger;
    // bool EndedTurn;
    public void HasEndedTurn()
    {
        // EndedTurn = true;
    }

    IEnumerator EndTurn()
    {
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        Horn.CanEndTurn = false;
        // EndedTurn = false;
        completeTutorialRoutine(EndTurn1st_Trigger, 2);
    }

    public TutorialMonster monster;
    public TutorialStepTrigger AttackAgain_Trigger;
    public DeckController deckController;
    IEnumerator AttackAgain()
    {
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.CombatEnemyStep);
        deckController.clear();
        CardPlayData defenceCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Shield/Defence_BattlleWornShield_CardSO"));
        deckController.AddCardToLib(defenceCard);
        CardPlayData buffCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Urdarbrunnr_CardSO"));
        deckController.AddCardToLib(buffCard);
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        // EndedTurn = false;
        Horn.CanEndTurn = false;
        completeTutorialRoutine(AttackAgain_Trigger, 3);
    }

    public TutorialStepTrigger defend_Trigger;
    IEnumerator DefendYourself()
    {
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.AttackPlayer);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.CombatEnemyStep);
        deckController.clear();
        CardPlayData defenceCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_Gramr_CardSO"));
        deckController.AddCardToLib(defenceCard);
        CardPlayData buffCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Urdarbrunnr_CardSO"));
        deckController.AddCardToLib(buffCard);
        CardPlayData GodCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Gods/God_Tyr"));
        deckController.AddCardToLib(GodCard);
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        completeTutorialRoutine(defend_Trigger, 4);
    }
}
