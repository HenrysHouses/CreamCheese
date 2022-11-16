/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools.Examples;

public class TurnController : CombatFSM
{
    // * Combat Mechanic references
    [SerializeField] BoardStateController BoardStateController;
    [SerializeField] PlayerTracker player;
    [SerializeField] GodPlacement godPlace;
    [SerializeField] DeckManager_SO deckManager;
    [SerializeField] PathAnimatorController DiscardAnimator;
    [SerializeField] PathAnimatorController DrawAnimator;
    [SerializeField] PathAnimatorController ShuffleAnimator;
    [SerializeField] SceneTransition SceneTransition;

    // * Combat Variables

    /// <summary>How many cards the player draws at their draw step</summary>
    public int DrawStepCardAmount = 5;
    public Player_Hand _Hand;
    public bool isDiscardAnimating => DiscardAnimator.isAnimating;
    public bool isDrawAnimating => DrawAnimator.isAnimating;
    public bool isShuffleAnimating => ShuffleAnimator.isAnimating;
    public bool isCombatStarted = false;
    public bool shouldEndTurn = false;
    public static bool shouldWaitForAnims = false;
    [SerializeField] bool waitForLibraryShuffle = false;

    public CombatState state;

    Card_Behaviour selectedCard;

    //particle effect
    [SerializeField] SlashParticles slashParticles;

    protected override void Initialize()
    {
        deckManager.SetCurrentDeck(player.CurrentDeck);
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

        EndState _endStep = new EndState(this,player);
        _endStep.AddTransition(Transition.EnterDraw, CombatState.DrawStep);

        AddFSMState(_combatSetup);
        AddFSMState(_drawStep);
        AddFSMState(_mainPhase);
        AddFSMState(_discardStep);
        AddFSMState(_combatStepStart);
        AddFSMState(_combatCard);
        AddFSMState(_combatEnemy);
        AddFSMState(_endStep);
    }

    internal void GodDied(God_Behaviour god)
    {
        //Animation of god dying????

        Destroy(god.transform.parent.parent.gameObject, 1);

        _Hand.RemoveCard(god.GetComponent<Card_Loader>());

        God_Card_SO _Card_SO = BoardStateController.playedGodCard.CardSO;
        _Card_SO.StartDialogue(GodDialogueTrigger.Dying, _Card_SO);
        BoardStateController.playedGodCard = null;
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    protected override void FSMUpdate()
    {
        if(BoardStateController.isEncounterInstantiated)
        {
            CurrentState.Reason();
            CurrentState.Act();
        }

        state = CurrentStateID;

        if(!SceneTransition.isTransitioning && !BoardStateController.isEncounterInstantiated)
        {
            BoardStateController.spawnEncounter();
        }
    }

    // * --- Sound Management ---

    void CardSound(EventReference event_, GameObject target)
    {
        SoundPlayer.PlaySound(event_, target);
    }

    // * --- Turn Management ---

    public void EndTurn()
    {
        if (CurrentState is MainState)
        {
            shouldEndTurn = true;
            if (selectedCard)
            {
                selectedCard.CancelSelection();
            }
        }
    }


    // * --- Card management ---

    /// <summary>Draw cards for the player</summary>
    /// <param name="amount">The amount of cards the player should draw</param>
    public void Draw(int amount) => StartCoroutine(cardDrawTrigger(amount));
    IEnumerator cardDrawTrigger(int amount)
    {
        // wait until the discard has been shuffled into the library before drawing cards
        yield return new WaitUntil(() => !ShuffleAnimator.isAnimating);


        CardPathAnim[] animData = deckManager.drawCard(amount, 0.25f);

        // if(animData != null)
        //     Debug.Log("draw: " + animData.Length + " - " + amount);

        // # if trigger data == null, then there was not enough cards in library to draw
        if(animData != null) 
        {
            animData[animData.Length - 1].OnAnimCompletionTrigger.AddListener(animsAreDone);
            animData[animData.Length - 1].OnAnimCompletionTrigger.AddListener(SetCards);
            

            foreach (var trigger in animData)
            {
                if(trigger is null)
                    continue;

                trigger.OnCardCompletionTrigger.AddListener(_Hand.AddCard);
                trigger.OnAnimStartSound.AddListener(CardSound);


                // Dialogue draw trigger
                if(trigger.cardSO is God_Card_SO _God)
                {
                    trigger.OnCardDrawDialogue.AddListener(_God.StartDialogue);
                }
                else if(BoardStateController.playedGodCard is not null)
                {
                    God_Card_SO _God_SO = BoardStateController.playedGodCard.CardSO;
                    trigger.OnCardDrawDialogue.AddListener(_God_SO.StartDialogue);
                }   
            }

            yield break; // stops Coroutine here
        }
        
        ShuffleDiscard(amount); // Does not let the player draw the remaining cards and then shuffle
    }

    void SetCards()
    {
        foreach (Player_Hand.CardHandAnim card in _Hand.CardSelectionAnimators)
        {
            card.loader.Behaviour.SetController(this);
        }
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

        CardPathAnim lastAnim = null;


        if (_Hand.CardSelectionAnimators.Count > 0)
        {
            for (int i = _Hand.CardSelectionAnimators.Count - 1; i >= 0; i--)
            {
                lastAnim = deckManager.discardCard(_Hand.CardSelectionAnimators[i].cardSO);

                var selectorParentToDestroy = _Hand.CardSelectionAnimators[i].Selector.transform.parent.gameObject;
                
                Card_Loader _Loader = _Hand.CardSelectionAnimators[i].loader; 
                
                _Hand.RemoveCard(_Loader);
                Destroy(selectorParentToDestroy);
                yield return new WaitForSeconds(delayBetweenCards);

                // Dialogue discard trigger
                if(BoardStateController.playedGodCard is not null)
                    lastAnim.OnCardDrawDialogue.AddListener(BoardStateController.playedGodCard.CardSO.StartDialogue);
                else if(_Loader.GetCardSO.type == CardType.God)
                {
                    God_Card_SO _God = _Loader.GetCardSO as God_Card_SO;
                    lastAnim.OnCardDrawDialogue.AddListener(_God.StartDialogue);
                }

            }

            lastAnim.OnAnimCompletionTrigger.AddListener(animsAreDone);
        }
        else
        {
            animsAreDone();
        }
    }

    public void Discard(Card_Behaviour card, float delay = 0f) => discardTrigger(card, delay);
    void discardTrigger(Card_Behaviour card_b, float delayBetweenCards)
    {
        //yield return new WaitUntil(() => !DrawAnimator.isAnimating && !ShuffleAnimator.isAnimating);

        // CardPathAnim lastAnim = null;

        if (card_b != null)
        {
            CardPathAnim anim = deckManager.discardCard(card_b.GetComponent<Card_Loader>().GetCardSO);

            // Dialogue discard trigger
            if(BoardStateController.playedGodCard is not null)
                anim.OnCardDrawDialogue.AddListener(BoardStateController.playedGodCard.CardSO.StartDialogue);
            //lastAnim.OnAnimCompletionTrigger.AddListener(animsAreDone);
            _Hand.RemoveCard(card_b.GetComponent<Card_Loader>());

            Destroy(card_b.transform.parent.parent.gameObject);
        }
        else
        {
            animsAreDone();
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
    public BoardStateController GetBoard() { return BoardStateController; }

    public Card_Behaviour SelectedCard => selectedCard;
    public GodPlacement GodPlacement => godPlace;

    // * --- Clicking --- 
    public void SetSelectedCard(Card_Behaviour sel = null)
    {
        if (sel)
        {
            if (selectedCard && selectedCard != sel)
                selectedCard.CancelSelection();
        }
        selectedCard = sel;
    }

    public static BoardElement PlayerClick()
    {
        LayerMask layerMask = LayerMask.GetMask("Board Element", "Card");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        BoardElement element = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, ~layerMask) && Input.GetMouseButtonDown(0))
        {
            GameObject clicked = hit.collider.gameObject;

            element = clicked.GetComponent<BoardElement>();

            if (element && element.OnClick())
                return element;
        }
        return null;
    }
}