/* 
 * Written by 
 * Henrik
 * 
 * modified by charlie and Javier
*/

using System.Collections;
using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools.Examples;
using HH.MultiSceneTools;

public class TurnController : CombatFSM
{
    // * Combat Mechanic references
    [SerializeField] BoardStateController BoardStateController;
    [field:SerializeField] public PlayerTracker player {get; private set;}
    [SerializeField] GodPlacement godPlace;
    [SerializeField] DeckController deckController;
    [SerializeField] PathAnimatorController DiscardAnimator;
    [SerializeField] PathAnimatorController DrawAnimator;
    [SerializeField] PathAnimatorController ShuffleAnimator;
    [SerializeField] SceneTransition _SceneTransition;
    [SerializeField] PathController DrawAnimController, DiscardAnimController;
    [SerializeField] Transform DrawEndPoint;
    [SerializeField] Transform DiscardStartPoint;
    [SerializeField] Transform[] DiscardPositions;

    [SerializeField] public DeathCrumbling death;

    // * Combat Variables

    /// <summary>How many cards the player draws at their draw step</summary>
    [SerializeField]
    public int DrawStepCardAmount = 7;
    [SerializeField]
    private float drawDelay;
    [HideInInspector]
    public int DrawCardExtra = 0;
    // public DeckManager_SO _DeckManager => deckManager;
    public Player_Hand _Hand;
    public bool isDiscardAnimating => DiscardAnimator.isAnimating;
    public bool isDrawAnimating => DrawAnimator.isAnimating;
    public bool isShuffling = false, isDrawing = false, isDiscarding = false;
    public bool isCombatStarted = false;
    public bool shouldEndTurn = false;
    public static bool shouldWaitForAnims;
    private float failSafeTimer, failSafeThreshold = 20;
    [SerializeField] bool waitForLibraryShuffle = false;
    CardPathAnim[] CardAnimations;

    public CombatState state;

    Card_Behaviour selectedCard;

    protected override void Initialize()
    {
        deckController.setCurrentDeck(player.CurrentDeck);

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

        CameraMovement.instance.SetCameraView(CameraView.Reset);
    }

    internal void GodDied(GodCard_Behaviour god)
    {
        //Animation of god dying????
        StartCoroutine(god.GetComponent<Card_Loader>().DissolveCard(0.5f));

        Destroy(god.transform.parent.gameObject, 4.4f);

        _Hand.RemoveCard(god.GetComponent<Card_Loader>());

        GodCard_ScriptableObject _Card_SO = BoardStateController.playedGodCard.CardSO;
        _Card_SO.StartDialogue(GodDialogueTrigger.Dying, _Card_SO);
        BoardStateController.playedGodCard = null;
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    protected override void FSMUpdate()
    {
        CheckFailSafe();

        if(BoardStateController.isEncounterInstantiated)
        {
            CurrentState.Reason();
            CurrentState.Act();
        }

        state = CurrentStateID;

        if(!_SceneTransition.isTransitioning && !BoardStateController.isEncounterInstantiated)
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

    /// <summary>Checks if too much time passes between animations and skips them</summary>
    private void CheckFailSafe()
    {
        if(!shouldWaitForAnims)
        {
            failSafeTimer = 0;
            return;
        }
        
        if(shouldWaitForAnims)
            failSafeTimer += Time.deltaTime;

        if(failSafeTimer <= 0)
            return;

        if(failSafeTimer < failSafeThreshold)
            return;

        failSafeTimer = 0;

        StopAllCoroutines();
        shouldWaitForAnims = false;
        BoardStateController.removeNullEnemies(); // ! this may be bad idk yet
        Debug.Log("fail safe trigger");
    }

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

    public void PlayerDying()
    {
        death.dying = true;
    }

    public IEnumerator ExitCombat()
    {
        deckController.clear();
        yield return new WaitForSeconds(6);
        _SceneTransition.TransitionScene(false, "Map");
        // MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
        Map.Map_Manager.SavingMap();
    }


    // * --- Card management ---

    /// <summary>Draw cards for the player</summary>
    /// <param name="amount">The amount of cards the player should draw</param>
    public void Draw(int amount) 
    {
        StartCoroutine(cardDrawTrigger(amount));
    } 
    IEnumerator cardDrawTrigger(int amount)
    {
        isDrawing = true;
        // wait until the discard has been shuffled into the library before drawing cards
        yield return new WaitUntil(() => !ShuffleAnimator.isAnimating);
        yield return new WaitUntil(() => CardAnimations == null);


        CardAnimations = deckController.drawCard(amount, drawDelay);
        // if(animData != null)
        //     Debug.Log("draw: " + animData.Length + " - " + amount);

        // # if trigger data == null, then there was not enough cards in library to draw
        if(CardAnimations != null) 
        {
            CardAnimations[CardAnimations.Length - 1].OnAnimCompletionTrigger.AddListener(animsAreDone);
            CardAnimations[CardAnimations.Length - 1].OnAnimCompletionTrigger.AddListener(SetCards);
            
            foreach (var trigger in CardAnimations)
            {
                if(trigger is null)
                    continue;

                trigger.OnCardCompletionTrigger.AddListener(_Hand.AddCard);
                trigger.OnAnimStartSound.AddListener(CardSound);


                // Dialogue draw trigger
                if(trigger._card.CardType is GodCard_ScriptableObject _God)
                {
                    trigger.OnCardDrawDialogue.AddListener(_God.StartDialogue);
                }
                else if(BoardStateController.playedGodCard is not null)
                {
                    GodCard_ScriptableObject _God_SO = BoardStateController.playedGodCard.CardSO;
                    trigger.OnCardDrawDialogue.AddListener(_God_SO.StartDialogue);
                }   
            }
            isDrawing = false;
            yield break; // stops Coroutine here
        }
        
        isDrawing = false;
        if(!isShuffling && !ShuffleAnimator.isAnimating)
        {
            ShuffleDiscard(amount); // Does not let the player draw the remaining cards and then shuffle
        }
    }

    void SetCards()
    {
        foreach (Player_Hand.CardHandAnim card in _Hand.CardSelectionAnimators)
        {
            card.loader.Behaviour.SetController(this);
        }
    }

    public void ShuffleLibrary() => deckController.shuffleLibrary();

    /// <summary>Shuffles the discard into the library for the player</summary>
    /// <param name="drawAmount">Amount of cards that should be drawn after shuffling the discard</param>
    public void ShuffleDiscard(int drawAmount = 0) 
    {
        if(!isShuffling) StartCoroutine(shuffleDiscardTrigger(drawAmount));
    } 
    /// <summary>Moves cards to library and shuffles, Requests animations for each card that was shuffled</summary>
    IEnumerator shuffleDiscardTrigger(int drawAfterShuffle)
    {
        isShuffling = true;
        yield return new WaitUntil(() => !DrawAnimator.isAnimating);
        yield return new WaitUntil(() => CardAnimations == null);

        CardAnimations = deckController.shuffleDiscard(drawDelay);

        if(CardAnimations.Length == 0)
        {
            isShuffling = false;
            yield break;
        }

        foreach (CardPathAnim trigger in CardAnimations)
        {
            trigger.OnAnimStartSound.AddListener(CardSound);
        }

        if(drawAfterShuffle <= 0) // stops animations here
        {
            CardAnimations[CardAnimations.Length-1].OnAnimCompletionTrigger.AddListener(animsAreDone);
        }
        else
        {
            waitForLibraryShuffle = true;
            // if(animData.Length-1 > -1)

            CardAnimations[CardAnimations.Length-1].OnAnimCompletionTrigger.AddListener(waitForShuffleAnims);

            yield return new WaitUntil(() => !waitForLibraryShuffle);
            Draw(drawAfterShuffle);
            // Debug.Log("DRAW after shuffle");
        }
        isShuffling = false;
    }

    /// <summary>Discards cards then requests animations for each discarded card with a delay between each</summary>
    public void DiscardAll() 
    {
        if(!isDiscarding) StartCoroutine(discardAllTrigger(drawDelay));
    }
    IEnumerator discardAllTrigger(float delayBetweenCards)
    {
        isDiscarding = true;
        yield return new WaitUntil(() => !DrawAnimator.isAnimating && !ShuffleAnimator.isAnimating);
        yield return new WaitUntil(() => CardAnimations == null);

        CardPlayData[] _data = deckController.GetHand();

        if (_data.Length > 0)
        {
            Vector3 position = _Hand.CardSelectionAnimators[_Hand.CardSelectionAnimators.Count-1].loader.transform.position;
            setDiscardPathToHandPosition(position);
            CardAnimations = deckController.discardCard(_data, delayBetweenCards);
            
            for (int i = _data.Length - 1; i >= 0; i--)
            {
                CardAnimations[i].OnAnimStartSound.AddListener(CardSound);

            
                var selectorParentToDestroy = _Hand.CardSelectionAnimators[i].Selector.transform.parent.gameObject;

                Card_Loader _Loader = _Hand.CardSelectionAnimators[i].loader; 

                _Hand.RemoveCard(_Loader);
                Destroy(selectorParentToDestroy, delayBetweenCards*i);
                // yield return new WaitForSeconds(delayBetweenCards);

                // Dialogue discard trigger
                if(BoardStateController.playedGodCard is not null)
                {
                    CardAnimations[i].OnCardDrawDialogue.AddListener(BoardStateController.playedGodCard.CardSO.StartDialogue);

                }
                else if(_Loader.GetCardSO.type == CardType.God)
                {
                    GodCard_ScriptableObject _God = _Loader.GetCardSO as GodCard_ScriptableObject;
                    CardAnimations[i].OnCardDrawDialogue.AddListener(_God.StartDialogue);
                }

            }

            CardAnimations[CardAnimations.Length-1].OnAnimCompletionTrigger.AddListener(animsAreDone);
        }
        else
        {
            animsAreDone();
        }
        isDiscarding = false;
    }

    /// <summary>Discards cards then requests animations for each discarded card with a delay between each</summary>
    public void Discard(Card_Behaviour card, float delay = 0f)
    {
        if(!isDiscarding) discardTrigger(card, delay);
    }
    IEnumerator discardTrigger(Card_Behaviour card_b, float delayBetweenCards)
    {
        isDiscarding = true;
        yield return new WaitUntil(() => !DrawAnimator.isAnimating && !ShuffleAnimator.isAnimating);
        yield return new WaitUntil(() => CardAnimations == null);


        // CardPathAnim lastAnim = null;
        Debug.Log(card_b);
        Transform ClosestPos = DiscardPositions[0];
        foreach (var pos in DiscardPositions)
        {
            if(Vector3.Distance(ClosestPos.position, card_b.transform.position) > Vector3.Distance(pos.position, card_b.transform.position))
                ClosestPos = pos;
        }
        DiscardStartPoint.position = ClosestPos.position;
        DiscardStartPoint.rotation = ClosestPos.rotation;
        DiscardStartPoint.localScale = ClosestPos.localScale;
        DiscardAnimController.recalculatePath();
        Debug.Log(DiscardStartPoint.name);

        if (card_b != null)
        {
            CardPlayData[] data = new CardPlayData[]{card_b.GetComponent<Card_Loader>()._card};
            CardAnimations = deckController.discardCard(data, delayBetweenCards);

            // Dialogue discard trigger
            if(BoardStateController.playedGodCard is not null && CardAnimations[0] is not null)
                CardAnimations[0].OnCardDrawDialogue.AddListener(BoardStateController.playedGodCard.CardSO.StartDialogue);

            CardAnimations[0].OnAnimCompletionTrigger.AddListener(animsAreDone);
            _Hand.RemoveCard(card_b.GetComponent<Card_Loader>());
            isDiscarding = false;
        }
        else
        {
            animsAreDone();
        }
        isDiscarding = false;
    }

    public void RemoveCardFromBoard(Card_Behaviour card_b)
    {
        // Figures out the discard position
        Transform ClosestPos = DiscardPositions[0];
        foreach (var pos in DiscardPositions)
        {
            if(Vector3.Distance(ClosestPos.position, card_b.transform.position) > Vector3.Distance(pos.position, card_b.transform.position))
                ClosestPos = pos;
        }
        DiscardStartPoint.position = ClosestPos.position;
        DiscardStartPoint.rotation = ClosestPos.rotation;
        DiscardStartPoint.localScale = ClosestPos.localScale;
        DiscardAnimController.recalculatePath();

        // Removes card from hand
        if (card_b != null)
        {
            // CardPlayData[] data = new CardPlayData[]{card_b.GetComponent<Card_Loader>()._card};
            // CardAnimations = deckController.discardCard(data, delayBetweenCards, BoardStateController.ExhaustedCards);

            bool isExhausted = false;
            for (int i = 0; i < card_b.stats.actionGroup.actions.Count; i++)
            {
                if(card_b.stats.actionGroup.actions[i] is ExhaustCardAction)
                {
                    isExhausted = true;
                    card_b.stats.actionGroup.actions[i].playSFX(gameObject);
                    Debug.Log("Exhaust Animation here");
                }
            }

            CardPathAnim anim = deckController.DiscardCardOnBoard(card_b.getCardPlayData(), 0, isExhausted);

            // Dialogue discard trigger
            if(BoardStateController.playedGodCard is not null && anim is not null)
                anim.OnCardDrawDialogue.AddListener(BoardStateController.playedGodCard.CardSO.StartDialogue);

            _Hand.RemoveCard(card_b.GetComponent<Card_Loader>());
        }
    }

    void setDiscardPathToHandPosition(Vector3 position)
    {
        DiscardStartPoint.position = position;
        DiscardStartPoint.rotation = new Quaternion(0, -0.710754335f, 0, 0.703440309f);
        DiscardStartPoint.localScale = new Vector3(1, 1, 0.0614522696f);
        DiscardAnimController.recalculatePath();
    }

    void animsAreDone()
    {
        shouldWaitForAnims = false;
        CardAnimations = null;
    }

    void waitForShuffleAnims()
    {
        waitForLibraryShuffle = false;
        CardAnimations = null;
    }

    public void addExperience(CardStats card)
    {
        int ID = card.UpgradePath.Experience.ID;

        for (int i = 0; i < deckController.deckData.Count; i++)
        {
            ActionCard_ScriptableObject _Card = deckController.deckData.deckListData[i].CardType as ActionCard_ScriptableObject;

            if(_Card == null)
                continue;

            if(deckController.deckData.deckListData[i].Experience.ID != ID)
                continue;
            
            CardUpgradePath unlocks = _Card.cardStats.UpgradePath;
            CardPlayData _CardState = deckController.deckData.deckListData[i];
            if(unlocks.Upgrades == null)
            {
                Debug.Log("this card has no upgrades");
                return;
            }

            if(unlocks.Upgrades.Length < 1)
            {
                Debug.Log("this card has no upgrades");
                return;
            }

            if(unlocks.Experience.Level == unlocks.Upgrades.Length-1)
            {
                Debug.Log("this card is max level");
                return;
            }

            Debug.Log(_Card.cardName + ", ID: " + _CardState.Experience.ID + " just got more experience! at index " + i + " in the deckmanager deck. Previous XP: " + _CardState.Experience.XP);
            _CardState.Experience.XP++;
            card.UpgradePath.Experience.XP++;

            deckController.deckData.deckListData[i] = new CardPlayData(_CardState);
            // Debug.Log(deckManager.getDeck.deckData.deckListData[i].Experience.XP);

            if(_CardState.Experience.XP >= unlocks.Upgrades[_CardState.Experience.Level].RequiredXP)
            {
                Debug.Log("level up");
                _CardState.Experience.Level++;
                card.UpgradePath.Experience.Level++;
                deckController.deckData.deckListData[i] = new CardPlayData(_CardState);
            }
            deckController.TransferExperienceToHand(_CardState);
        }
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

    public void resetSelectedCard()
    {
        selectedCard = null;
    }

    public static BoardElement PlayerClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        BoardElement element = null;
        
        RaycastHit[] hits = Physics.RaycastAll(ray, 10000);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if(!hits[i].collider.GetComponent<BoardElement>())
                    continue;

                GameObject clicked = hits[i].collider.gameObject;

                element = clicked.GetComponent<BoardElement>();

                if(!element)
                    return null;

                return element;
            }
        }
        return null;
    }
}