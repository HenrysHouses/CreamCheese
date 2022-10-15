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
    public PathAnimatorController DrawAnimator;
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

    public CombatState state;

    protected override void Initialize()
    {
        deckManager.SetCurrentDeck(player.Deck);
        deckManager.reset();

        AwakeCombatState _combatSetup = new AwakeCombatState(this);
        _combatSetup.AddTransition(Transition.EnterDraw, CombatState.DrawStep);

        DrawState _drawStep = new DrawState(this);
        _drawStep.AddTransition(Transition.EnterDiscard, CombatState.DiscardStep); // ! should be main phase, but is discard for testing 

        DiscardState _discardStep = new DiscardState(this);
        _discardStep.AddTransition(Transition.EnterEnd, CombatState.EndStep);

        EndState _endStep = new EndState(this);
        _endStep.AddTransition(Transition.EnterDraw, CombatState.DrawStep);

        // TODO
        AddFSMState(_combatSetup);
        AddFSMState(_drawStep);
        // AddFSMState(_mainPhase);
        AddFSMState(_discardStep);
        // AddFSMState(_CombatStepStart);
        // AddFSMState(_CombatCard);
        // AddFSMState(_CombatEnemy);
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

    void checkTurnStatus()
    {
        
    }

    void EnterCombatTrigger()
    {
        Draw(DrawStepCardAmount);
    }

    public void EndTurn()
    {
        if(CurrentState.Equals(CombatState.MainPhase))
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

        UnityEvent<Card_SO>[] triggerData = deckManager.drawCard(amount);
        triggerData[triggerData.Length-1].AddListener(animsAreDone);
        
        foreach (var trigger in triggerData)
        {
            if(trigger != null)
                trigger.AddListener(_Hand.AddCard);
        }
    }

    public void Shuffle() => deckManager.shuffleLibrary();

    public void DiscardAll() => deckManager.discardAll();

    void animsAreDone(Card_SO so)
    {
        shouldWaitForAnims = false;
    }
}