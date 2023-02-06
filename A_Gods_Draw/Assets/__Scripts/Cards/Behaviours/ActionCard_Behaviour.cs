// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class ActionCard_Behaviour : Card_Behaviour
{
    protected new ActionCard_ScriptableObject card_so;
    public Transform RootTransform;
    CardType cardType; 
    [SerializeField]
    EventReference SoundClick;
    BoardElement target;
    List<BoardElement> SelectedTargets = new List<BoardElement>();
    public BoardElement[] AllTargets => SelectedTargets.ToArray();
    Coroutine onSelectedRoutine;
    Coroutine actionRoutine;
    bool hasClickedTarget, cardIsReady, missedClick, IsOnBoard;
    public bool CardIsPlaced => IsOnBoard;
    public int neededLanes = 1;
    public CardStats stats;
    ActionGroup _actionGroup {get => stats.actionGroup; set{stats.actionGroup = value;}}
    ActionGroup _godBuffActions {get => stats.godBuffActions; set{stats.godBuffActions = value;}}

    public CardType GetCardType => cardType;
    public new ActionCard_ScriptableObject CardSO => card_so;

    private IEnumerator SelectingTargets()
    {
        CheckForGod();

        CameraMovement.instance.SetCameraView(stats.TargetingView);
        // int[] UniqueTargets = hasUniqueTargets(stats.actionGroup.actionStats.ToArray());
        
        for (int i = 0; i < stats.numberOfTargets; i++)
        {
            hasClickedTarget = false;
            missedClick = false;
            yield return new WaitUntil(() => hasClickedTarget);
            
            Debug.LogWarning("This needs to be changed, depending on card stats this may be out of range");


            if(!IsValidSelection(target, stats.SelectionType))
            {
                MissClick();
                yield break;    
            }
                
            SelectedTargets.Add(target);

            target = null;
        }
        
        CameraMovement.instance.ResetView();
        cardIsReady = true;
        controller.GetBoard().PlayCard(this);
        TurnController.shouldWaitForAnims = false;
    }

    /// <summary>Finds actions with required unique targets</summary>
    /// <param name="Actions">All the actions to check for unique targets</param>
    /// <returns>Indices of the actions that requires unique targets</returns>
    // private int[] hasUniqueTargets(CardActionData[] Actions)
    // {
    //     List<int> indices = new List<int>();

    //     for (int i = 0; i < Actions.Length; i++)
    //     {
    //         if(Actions[i].SelectionOverride.Index != 0)
    //             indices.Add(i);
    //     }
    //     return indices.ToArray();
    // }

    private bool IsValidSelection(BoardElement target, CardSelectionType selectionType)
    {
        string targetClassName = target.ClassName;

        // Debug.Log(targetClassName);

        if(targetClassName.Equals("None"))
            return false;

        if(targetClassName.Equals("BoardElement"))
            return true;

        int monsterIndex = BoardElementClassNames.instance.getIndexOf("Monster"); 
        int targetIndex = BoardElementClassNames.instance.getIndexOf(targetClassName);

        if(monsterIndex == selectionType.Index)
        {
            if(targetClassName.Contains("Monster"))
                return true;
        }

        int ActionCardIndex =  BoardElementClassNames.instance.getIndexOf("ActionCard_Behaviour");

        if(ActionCardIndex == selectionType.Index)
        {
            ActionCard_Behaviour card = target as ActionCard_Behaviour;

            if(card)
            {
                if(card.CardIsPlaced)
                {
                    return true;
                }
                return false;
            }
        }

        if(targetIndex == selectionType.Index)
            return true;
        return false;
    }

    private bool AllActionsReady()
    {
        return cardIsReady && onPlayerHand;
    }
    public void MissClick()
    {
        missedClick = true;
        CancelSelection();
    }

    public void Initialize(ActionCard_ScriptableObject card, CardElements elements)
    {
        ClassName = "ActionCard_Behaviour";
        RootTransform = transform.parent;
        this.card_so = card;
        stats = card.cardStats.Clone();

        for (int i = 0; i < _actionGroup.actionStats.Count; i++)
        {
            CardAction act = GetAction(_actionGroup.actionStats[i]);

            act.action_SFX = _actionGroup.actionStats[i].action_SFX;
            // act.PlayOnPlacedOrTriggered_SFX = _actionGroup.actionStats[i].PlayOnPlacedOrTriggered_SFX;
            act._VFX = _actionGroup.actionStats[i]._VFX;

            act.SetBehaviour(this);
            _actionGroup.Add(act); 
        }

        for (int i = 0; i < _godBuffActions.actionStats.Count; i++)
        {
            var act = GetAction(_godBuffActions.actionStats[i]);
            
            act.action_SFX = _godBuffActions.actionStats[i].action_SFX;
            // act.PlayOnPlacedOrTriggered_SFX = _godBuffActions.actionStats[i].PlayOnPlacedOrTriggered_SFX;
            act.SetBehaviour(this);
            _godBuffActions.Add(act);
        }

        this.cardType = card.type;
        this.elements = elements;
    }
    public void Buff(int value, bool isMult)
    {
        if(!IsOnBoard)
            return;

        if(isMult)
            stats.strength *= value;
        else
            stats.strength += value;
        elements.strength.text = stats.strength.ToString();
    }

    public void DeBuff(int value, bool isDivided)
    {
        if(!IsOnBoard)
            return;

        if(isDivided)
            stats.strength /= value;
        else
            stats.strength -= value;
    }
    public bool CheckForGod()
    {
        if (!controller.GetBoard().playedGodCard)
            return false;

        if (stats.correspondingGod != controller.GetBoard().playedGodCard.CardSO.godAction)
            return false;

        // controller.GetBoard().playedGodCard.Buff(this);
        Debug.LogWarning("something funky might be here");
        return true;
    }
    public void RemoveFromHand()
    {
        controller.Discard(this);
    }

    protected override void OnBeingSelected()
    {
        if (onSelectedRoutine == null)
        {
            if (!CanBeSelected())
            {
                CancelSelection();
                return;
            }
            TurnController.shouldWaitForAnims = true;
            onSelectedRoutine = StartCoroutine(SelectingTargets());
        }
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        
        for (int i = 0; i < _actionGroup.actions.Count; i++)
        {
            CardAction action = _actionGroup.actions[i];
            // BoardElement[] UniqueTargets = GetUniqueTargetsOf(i, SelectedTargets);

            StartCoroutine(action.OnAction(board, this));
            if (action.PlayOnPlacedOrTriggered_SFX)
            {
                SoundPlayer.PlaySound(action.action_SFX, gameObject);
            }
            yield return new WaitUntil(() => action.Ready);
        }

        if(CheckForGod())
        {
            foreach (var action in _godBuffActions.actions)
            {
                StartCoroutine(action.OnAction(board, this));
                if (action.PlayOnPlacedOrTriggered_SFX)
                {
                    SoundPlayer.PlaySound(action.action_SFX, gameObject);
                }
                yield return new WaitUntil(() => action.Ready);
            }
        }

        yield return new WaitForSeconds(0.2f);

        SelectedTargets.Clear();
        
        foreach (var action in stats.actionGroup.actions)
            action.Reset(board, this);
        foreach (var action in stats.godBuffActions.actions)
            action.Reset(board, this);

        controller.Discard(this);
        Destroy(transform.parent.gameObject);
        TurnController.shouldWaitForAnims = false;
    }

    internal override void OnClickOnSelected()
    {
        base.OnClickOnSelected();
        BoardElement element = TurnController.PlayerClick();

        if (element)
        {
            target = element;
            hasClickedTarget = true;
        }
        else
        {
            MissClick();
            hasClickedTarget = false;
        }
    }

    public override bool ShouldCancelSelection()
    {
        return this == null || missedClick;
    }
    public override bool CanBeSelected()
    {
        return base.CanBeSelected() && (controller.GetBoard().thingsInLane.Count + neededLanes <= 4);
    }
    public override bool CardIsReady()
    {
        return AllActionsReady();
    }
    protected override void OnPlacedInLane()
    {
        foreach (var target in SelectedTargets)
        {
            foreach (var action in _actionGroup.actions)
            {
                action.OnLanePlaced(controller.GetBoard(), this);
            }
            if (CheckForGod())
            {
                foreach (var action in _godBuffActions.actions)
                {
                    action.OnLanePlaced(controller.GetBoard(), this);
                }
            }
        }

        controller.PlacedCard();
        IsOnBoard = true;
        missedClick = true;
    }
    public override void OnAction()
    {
        TurnController.shouldWaitForAnims = true;

        Debug.Log("Errors happen here");

        StartCoroutine(Play(controller.GetBoard()));
    }
    public override void CancelSelection()
    {
        base.CancelSelection();
        if (onSelectedRoutine != null)
            StopCoroutine(onSelectedRoutine);
        if (actionRoutine != null)
            StopCoroutine(actionRoutine);
        onSelectedRoutine = null;
        actionRoutine = null;
        target = null;

        StopAllCoroutines();

        // foreach (var target in SelectedTargets.Targets)
        // {
        //     foreach (var action in _actionGroup.actions)
        //     {
        //         action.Reset(controller.GetBoard(), this);
        //     }
        //     foreach (var action in _godBuffActions.actions)
        //     {
        //         action.Reset(controller.GetBoard(), this);
        //     }
        // }
    }

    void ProgressUpgrade()
    {
        
    }
}
