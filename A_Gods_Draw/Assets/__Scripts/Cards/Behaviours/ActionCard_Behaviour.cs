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
    bool hasClickedTarget = false;
    bool cardIsReady = false;
    bool missedClick = false;
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
        // _action.SetClickableTargets(controller.GetBoard(), true);
        
        for (int i = 0; i < stats.numberOfTargets; i++)
        {
            hasClickedTarget = false;
            missedClick = false;
            yield return new WaitUntil(() => hasClickedTarget);
            
            SelectedTargets.Add(target);
            // foreach (var _thisAction in _actionGroup.actions)
            // {
            //     _thisAction.AddTarget(target);
            // }
            target = null;
            stats.actionGroup.actions[i].OnActionReady(controller.GetBoard(), this);
        }
        
        // _action.SetClickableTargets(controller.GetBoard(), false);
        CameraMovement.instance.ResetView();
        cardIsReady = true;
        controller.GetBoard().PlayCard(this);
        TurnController.shouldWaitForAnims = false;
    }
    private bool AllActionsReady()
    {
        return cardIsReady && onPlayerHand;
    }
    // private void RemoveGodBuff()
    // {
    //     if (godBuffed)
    //     {
    //         if (controller.GetBoard().playedGodCard)

    //         if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
    //         {
    //             controller.GetBoard().playedGodCard.DeBuff(this);
    //         }
    //         godBuffed = false;

    //         foreach (var godAction in godBuffActions)
    //             actionGroup.Remove(godAction);
    //     }
    // }

    public void MissClick()
    {
        missedClick = true;
        CancelSelection();
    }

    public void Initialize(ActionCard_ScriptableObject card, CardElements elements)
    {
        RootTransform = transform.parent;
        this.card_so = card;
        stats = card.cardStats.Clone();

        if(card.cardName == "Bifrost")
        {
            stats = stats;
        }

        for (int i = 0; i < _actionGroup.actionStats.Count; i++)
        {
            CardAction act = GetAction(_actionGroup.actionStats[i]);
            act.SetBehaviour(this);
            _actionGroup.Add(act);

            act.action_SFX = _actionGroup.actions[i].action_SFX;
            act.PlayOnPlacedOrTriggered_SFX = _actionGroup.actions[i].PlayOnPlacedOrTriggered_SFX;
            act._VFX = _actionGroup.actions[i]._VFX;
        }

        for (int i = 0; i < _godBuffActions.actionStats.Count; i++)
        {
            var act = GetAction(_godBuffActions.actionStats[i]);
            act.SetBehaviour(this);
            _godBuffActions.Add(act);
            
            act.action_SFX = _godBuffActions.actions[i].action_SFX;
            act.PlayOnPlacedOrTriggered_SFX = _godBuffActions.actions[i].PlayOnPlacedOrTriggered_SFX;
        }

        this.cardType = card.type;
        this.elements = elements;
    }
    public void Buff(int value, bool isMult)
    {
        if(isMult)
            stats.strength *= value;
        else
            stats.strength += value;
        elements.strength.text = stats.strength.ToString();
    }

    public void DeBuff(int value, bool isDivided)
    {
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
    // public BoardElement[] getActionTargets(int action)
    // {
    //     return _actionGroup[action].actions[0].targets.ToArray();
    // }


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
        foreach (var target in SelectedTargets)
        {
            foreach (var action in _actionGroup.actions)
            {
                StartCoroutine(action.OnAction(board, this));
                if (action.PlayOnPlacedOrTriggered_SFX)
                {
                    SoundPlayer.PlaySound(action.action_SFX, gameObject);
                }
                yield return new WaitUntil(() => action.Ready());
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
                    yield return new WaitUntil(() => action.Ready());
                }
            }
        }

        yield return new WaitForSeconds(0.2f);


        // foreach (var action in _actionGroup.actions)
        // {
        //     action.Reset(board, this);
        // }

        // foreach (var action in _godBuffActions.actions)
        // {
        //     action.Reset(board, this);
        // }
        Reset();

        controller.Discard(this);
        Destroy(transform.parent.gameObject);
        TurnController.shouldWaitForAnims = false;
    }

    private void Reset() {
        SelectedTargets.Clear();
        // stats.actionGroup.actions.Clear();    
        // stats.godBuffActions.actions.Clear();    
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
        //if (this == null)
        //return true;

        base.CancelSelection();
        if (onSelectedRoutine != null)
            StopCoroutine(onSelectedRoutine);
        if (actionRoutine != null)
            StopCoroutine(actionRoutine);
        onSelectedRoutine = null;
        actionRoutine = null;
        target = null;

        StopAllCoroutines();

        foreach (var target in SelectedTargets)
        {
            foreach (var action in _actionGroup.actions)
            {
                action.Reset(controller.GetBoard(), this);
            }
            foreach (var action in _godBuffActions.actions)
            {
                action.Reset(controller.GetBoard(), this);
            }
        }
    }
}
