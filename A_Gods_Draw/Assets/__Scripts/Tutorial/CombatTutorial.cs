using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorial : TutorialController
{
    protected override void Start()
    {
        GameManager.instance.nextCombatType = EncounterDifficulty.Tutorial;

        for (int i = 0; i < EnableBoard.Length; i++)
        {
            EnableBoard[i].SetActive(false);
        }
        Horn.CanEndTurn = false;

        base.Start();
    }

    public override void CheckTutorialConditions()
    {
        if (isTutorialStep(0))
            startTutorialRoutine(PressAnyButtonToContinue(Step1_Trigger));

        if (isTutorialStep(1))
            startTutorialRoutine(PlayAttackCardOnEnemy());

        if (isTutorialStep(2))
            startTutorialRoutine(EndTurn());

        if (isTutorialStep(3))
            startTutorialRoutine(AttackAgain());

        if (isTutorialStep(4))
            startTutorialRoutine(DefendYourself());
        if (isTutorialStep(5))
            startTutorialRoutine(GlyphExplain());
    }

    public GameObject[] EnableBoard;
    public TutorialStepTrigger Step1_Trigger;

    protected IEnumerator PressAnyButtonToContinue(TutorialStepTrigger trigger)
    {
        bool isAllowedToContinue = false;
        bool previousState = CurrentDialogue.fullPageIsDisplaying;

        while (isAllowedToContinue == false)
        {
            yield return new WaitForEndOfFrame();

            if (CurrentDialogue.fullPageIsDisplaying == previousState && CurrentDialogue.fullPageIsDisplaying)
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
        Horn.CanEndTurn = false;
        yield return new WaitUntil(() => turnController.GetBoard() != null);
        yield return new WaitUntil(() => turnController.isCombatStarted);
        yield return new WaitUntil(() => turnController.GetBoard().getLivingEnemies().Length > 0);
        monster = turnController.GetBoard().getLivingEnemies()[0] as TutorialMonster;
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.Defend);
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        deckController.clear();
        completeTutorialRoutine(PlayAttack_Trigger, 1);
    }

    public EndTurnButton Horn;
    public TutorialStepTrigger EndTurn1st_Trigger;
    // bool EndedTurn;

    IEnumerator EndTurn()
    {
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        Horn.CanEndTurn = false;
        // EndedTurn = false;
        deckController.clear();
        CardPlayData attackCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_Gramr_CardSO"));
        deckController.AddCardToLib(attackCard);
        CardPlayData buffCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Bifrost_CardSO"));
        deckController.AddCardToLib(buffCard);
        completeTutorialRoutine(EndTurn1st_Trigger, 2);
    }

    public TutorialMonster monster;
    public TutorialStepTrigger AttackAgain_Trigger;
    public DeckController deckController;
    IEnumerator AttackAgain()
    {

        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        deckController.clear();
        CardPlayData defenceCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Shield/Defence_BattlleWornShield_CardSO"));
        deckController.AddCardToLib(defenceCard);
        CardPlayData buffCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Bifrost_CardSO"));
        buffCard.Experience.XP = 11;
        buffCard.Experience.Level = 1;
        deckController.AddCardToLib(buffCard);
        Horn.CanEndTurn = false;
        completeTutorialRoutine(AttackAgain_Trigger, 3);
    }

    public TutorialStepTrigger defend_Trigger;
    IEnumerator DefendYourself()
    {
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.AttackPlayer);
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.CombatEnemyStep);
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        Horn.CanEndTurn = false;
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.AttackPlayer);
        deckController.clear();
        CardPlayData GodCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Gods/God_Tyr_CardSO"));
        deckController.AddCardToLib(GodCard);
        CardPlayData attackCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_Gramr_CardSO"));
        deckController.AddCardToLib(attackCard);

        completeTutorialRoutine(defend_Trigger, 4);

        StartCoroutine(PlayGodCard());
    }

    IEnumerator PlayGodCard()
    {
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Horn.CanEndTurn = true;
        // yield return new WaitUntil(() => turnController._Hand.isEmpty());
    }

    IEnumerator GlyphExplain()
    {
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.AttackPlayer);
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.CombatEnemyStep);
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        Horn.CanEndTurn = false;

    }
}
