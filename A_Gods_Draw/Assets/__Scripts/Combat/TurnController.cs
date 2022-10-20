/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnController : CombatFSM
{
    // * Combat Mechanic references
    [SerializeField] BoardStateController boardState;
    [SerializeField] PlayerTracker player;
    [SerializeField] DeckManager_SO deckManager;
    [SerializeField] PathAnimatorController DiscardAnimator;
    [SerializeField] PathAnimatorController DrawAnimator;
    [SerializeField] PathAnimatorController ShuffleAnimator;

    // * Combat Variables

    /// <summary>How many cards the player draws at their draw step</summary>
    public int DrawStepCardAmount = 5;
    public Player_Hand _Hand;
    public bool isDiscardAnimating => DiscardAnimator.isAnimating;
    public bool isDrawAnimating => DrawAnimator.isAnimating;
    public bool isShuffleAnimating => ShuffleAnimator.isAnimating;
    public bool isCombatStarted = false;
    public bool shouldEndTurn = false;
    public bool shouldWaitForAnims = false;
    [SerializeField] bool waitForLibraryShuffle = false;

    public CombatState state;

    protected override void Initialize()
    {
        deckManager.SetCurrentDeck(player.Deck);
        deckManager.reset();

        AwakeCombatState _combatSetup = new AwakeCombatState(this);
        _combatSetup.AddTransition(Transition.EnterDraw, CombatState.DrawStep);

        DrawState _drawStep = new DrawState(this);
        _drawStep.AddTransition(Transition.EnterMain, CombatState.MainPhase); 

        MainState _mainPhase = new MainState(this);
        _mainPhase.AddTransition(Transition.EnterDiscard, CombatState.DiscardStep); 

        DiscardState _discardStep = new DiscardState(this);
        _discardStep.AddTransition(Transition.EnterCombatStart, CombatState.CombatStartStep);

        StartState _combatStepStart = new StartState(this);
        _combatStepStart.AddTransition(Transition.EnterCombatCard, CombatState.CombatCardStep);

        CardsState _combatCard = new CardsState(this);
        _combatCard.AddTransition(Transition.EnterCombatEnemy, CombatState.CombatEnemyStep);

        EnemyState _combatEnemy = new EnemyState(this);
        _combatEnemy.AddTransition(Transition.EnterEnd, CombatState.EndStep);

        EndState _endStep = new EndState(this);
        _endStep.AddTransition(Transition.EnterDraw, CombatState.DrawStep);

        // TODO
        AddFSMState(_combatSetup);
        AddFSMState(_drawStep);
        AddFSMState(_mainPhase);
        AddFSMState(_discardStep);
        AddFSMState(_combatStepStart);
        AddFSMState(_combatCard);
        AddFSMState(_combatEnemy);
        AddFSMState(_endStep);
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    protected override void FSMUpdate()
    {
        if(boardState.isEncounterInstantiated)
        {
            CurrentState.Reason();
            CurrentState.Act();
        }

        state = CurrentStateID;
    }

    // * --- Turn Management ---

    public void EndTurn()
    {
        if(CurrentState is MainState)
            shouldEndTurn = true;
    }


    // * --- Card management ---

    /// <summary>Draw cards for the player</summary>
    /// <param name="amount">The amount of cards the player should draw</param>
    public void Draw(int amount) => StartCoroutine(cardDrawTrigger(amount));
    IEnumerator cardDrawTrigger(int amount)
    {
        // wait until the discard has been shuffled into the library before drawing cards
        yield return new WaitUntil(() => !ShuffleAnimator.isAnimating);

        Debug.Log("DRAW");

        CardPathAnim[] animData = deckManager.drawCard(amount, 0.25f);

        if(animData != null)
            Debug.Log("draw: " + animData.Length + " - " + amount);

        // # if trigger data == null, then there was not enough cards in library to draw
        if(animData != null) 
        {
            animData[animData.Length-1].OnAnimCompletionTrigger.AddListener(animsAreDone);

            foreach (var trigger in animData)
            {
                if(trigger != null)
                    trigger.OnCardCompletionTrigger.AddListener(_Hand.AddCard);
            }

            yield break; // stops Coroutine here
        }
        
        ShuffleDiscard(amount); // Does not let the player draw the remaining cards and then shuffle
    }

    public void ShuffleLibrary() => deckManager.shuffleLibrary();

    /// <summary>Shuffles the discard into the library for the player</summary>
    public void ShuffleDiscard(int drawAmount = 0) => StartCoroutine(shuffleDiscardTrigger(drawAmount));
    
    IEnumerator shuffleDiscardTrigger(int drawAfterShuffle)
    {
        yield return new WaitUntil(() => !DrawAnimator.isAnimating);

        CardPathAnim[] animData = deckManager.shuffleDiscard(0.18f);

        if(drawAfterShuffle <= 0) // stops animations here
        {
            animData[animData.Length-1].OnAnimCompletionTrigger.AddListener(animsAreDone);
        }
        else
        {
            waitForLibraryShuffle = true;
            animData[animData.Length-1].OnAnimCompletionTrigger.AddListener(waitForShuffleAnims);

            yield return new WaitUntil(() => !waitForLibraryShuffle);
            Draw(drawAfterShuffle);
            Debug.Log("DRAW after shuffle");
        }
    }

    public void DiscardAll(float delay) => StartCoroutine(discardAllTrigger(delay));
    IEnumerator discardAllTrigger(float delayBetweenCards)
    {
        yield return new WaitUntil(() => !DrawAnimator.isAnimating && !ShuffleAnimator.isAnimating);

        Debug.Log("DISCARD");
        CardPathAnim lastAnim = null;

        Card_SO[] cards  = deckManager.GetHandSO();
        for (int i = 0; i < cards.Length; i++)
        {
            lastAnim = deckManager.discardCard(cards[i]);
            if(i == cards.Length-1)
            {
                lastAnim.OnAnimCompletionTrigger.AddListener(animsAreDone);
                Debug.LogWarning("Discard animsAreDone() is not triggering");
            }
            
            _Hand.RemoveCard(0);
            yield return new WaitForSeconds(delayBetweenCards);
        }
    } 

    void animsAreDone()
    {
        shouldWaitForAnims = false;
    }

    void waitForShuffleAnims()
    {
        waitForLibraryShuffle = false;
        Debug.Log("ShuffleDone");
    }


    // * --- Getters ---

    public BoardStateController GetBoard() { return boardState; }
}