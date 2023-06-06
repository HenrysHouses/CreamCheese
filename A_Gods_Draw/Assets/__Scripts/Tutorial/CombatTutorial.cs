using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTutorial : TutorialController
{
    public GameObject tutorialStone;
    public GameObject textbox, tableController, hornMesh;
    protected override void Start()
    {
        textbox = GameObject.Find("TutorialTextBackground");
        // textBox.enabled = false;
        GameManager.instance.nextCombatType = EncounterDifficulty.Tutorial;

        for (int i = 0; i < EnableBoard.Length; i++)
        {

            EnableBoard[i].SetActive(false);
        }
        Horn.CanEndTurn = false;
        hornMesh.GetComponent<MeshRenderer>().enabled = false;

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
            startTutorialRoutine(PlayGodCard());

        if (isTutorialStep(6))
            startTutorialRoutine(GodGlyphsExplain());

        if (isTutorialStep(7))
            startTutorialRoutine(GlyphExplain());

        if (isTutorialStep(8))
            startTutorialRoutine(Utilitycards());

        if (isTutorialStep(9))
            startTutorialRoutine(Uppgrades());

        if (isTutorialStep(10))
            startTutorialRoutine(FinalStep());
    }

    public GameObject[] EnableBoard;
    public TutorialStepTrigger Step1_Trigger;

    protected IEnumerator PressAnyButtonToContinue(TutorialStepTrigger trigger)
    {
        textbox = GameObject.Find("TutorialTextBackground");

        // stoneAnim.SetBool("StoneIn", true);
        textbox.SetActive(true);
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
            // stoneAnim.SetBool("StoneIn", false);
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
        //  stoneAnim.SetBool("StoneIn", true);

        if (CurrentDialogue.fullPageIsDisplaying)
        {
            //   stoneAnim.SetBool("StoneIn", false);
        }

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
        hornMesh.GetComponent<MeshRenderer>().enabled = true;
        hornMesh.GetComponent<Animation>().Play();
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        Horn.CanEndTurn = false;
        // EndedTurn = false;
        deckController.clear();
        CardPlayData attackCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_TyrsBlessedSword_CardSO"));
        deckController.AddCardToLib(attackCard);
        Debug.Log("card name " + attackCard);
        CardPlayData buffCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Bifrost_CardSO"));
        deckController.AddCardToLib(buffCard);
       // monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.AttackPlayer);
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
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.AttackPlayer);
        deckController.clear();
        CardPlayData GodCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Gods/God_Tyr_CardSO"));
        deckController.AddCardToLib(GodCard);
        completeTutorialRoutine(defend_Trigger, 4);
        Horn.CanEndTurn = false;
        StartCoroutine(PlayGodCard());
    }

    IEnumerator PlayGodCard()
    {
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController._Hand.isEmpty());
        monster.CancelIntent();
        Horn.CanEndTurn = true;
        deckController.clear();
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);





        completeTutorialRoutine(defend_Trigger, 5);

        // yield return new WaitUntil(() => turnController._Hand.isEmpty());
    }

    IEnumerator GodGlyphsExplain()
    {
        deckController.clear();
        CardPlayData attackCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_TyrsBlessedSword_CardSO"));
        deckController.AddCardToLib(attackCard);
        CardPlayData defenceCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Shield/Defence_BattlleWornShield_CardSO"));
        deckController.AddCardToLib(defenceCard);
        CardPlayData buffCard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Bifrost_CardSO"));
        deckController.AddCardToLib(buffCard);
        // yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        completeTutorialRoutine(EndTurn1st_Trigger, 6);

    }

    IEnumerator GlyphExplain()
    {

        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.Defend);
        deckController.clear();
        CardPlayData buffcard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_SkinfaxiandHrimfaxi_CardSO"));
        deckController.AddCardToLib(buffcard);
        CardPlayData attackcard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_TyrsBlessedSword_CardSO"));
        deckController.AddCardToLib(attackcard);
        CardPlayData defencecard = new CardPlayData(Resources.Load<Card_SO>("Cards/Shield/Defence_BattlleWornShield_CardSO"));
        deckController.AddCardToLib(defencecard);
        Horn.CanEndTurn = true;

        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);



        completeTutorialRoutine(EndTurn1st_Trigger, 7);

    }

    IEnumerator Utilitycards()
    {

        deckController.clear();
        Horn.CanEndTurn = false;
        CardPlayData utility = new CardPlayData(Resources.Load<Card_SO>("Cards/Utility/Utility_HymirsCauldron_CardSO"));
        deckController.AddCardToLib(utility);
        CardPlayData attackcard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_TyrsBlessedSword_CardSO"));
        deckController.AddCardToLib(attackcard);
        yield return new WaitUntil(() => turnController.state == CombatState.DrawStep);
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        Horn.CanEndTurn = true;
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        completeTutorialRoutine(EndTurn1st_Trigger, 8);


    }


    IEnumerator Uppgrades()
    {


        deckController.clear();
        monster.TutorialIntentOverride(turnController.GetBoard(), TutorialActions.Defend);
        CardPlayData defencecard = new CardPlayData(Resources.Load<Card_SO>("Cards/Shield/Defence_BattlleWornShield_CardSO"));
        defencecard.Experience.XP = 4;
        deckController.AddCardToLib(defencecard);
        CardPlayData attack = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_TyrsBlessedSword_CardSO"));
        attack.Experience.XP = 9;
        deckController.AddCardToLib(attack);

        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);


        completeTutorialRoutine(EndTurn1st_Trigger, 9);



    }

    IEnumerator FinalStep()
    {
        yield return new WaitUntil(() => turnController.state == CombatState.EndStep);
        Horn.CanEndTurn = false;
        deckController.clear();
        CardPlayData utility = new CardPlayData(Resources.Load<Card_SO>("Cards/Utility/Utility_HymirsCauldron_CardSO"));
        deckController.AddCardToLib(utility);
        CardPlayData attackcard = new CardPlayData(Resources.Load<Card_SO>("Cards/Attack/Attack_TyrsBlessedSword_CardSO"));
        deckController.AddCardToLib(attackcard);
        deckController.AddCardToLib(attackcard);
        CardPlayData buffcard = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_SkinfaxiandHrimfaxi_CardSO"));
        CardPlayData buffCard1 = new CardPlayData(Resources.Load<Card_SO>("Cards/Buff/Buff_Bifrost_CardSO"));
        deckController.AddCardToLib(buffcard);
        deckController.AddCardToLib(buffCard1);
        yield return new WaitUntil(() => turnController.state == CombatState.MainPhase);
        Horn.CanEndTurn = true;
        textbox.SetActive(true);


    }
}
