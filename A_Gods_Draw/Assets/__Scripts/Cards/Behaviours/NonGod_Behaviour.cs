// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public struct ActionGroup
{
    public int nTargets;
    public List<CardAction> actions;

    public ActionGroup(int nOfTargets)
    {
        nTargets = nOfTargets;
        actions = new();
    }

    public CardAction this[int key]
    {
        get => actions[key];
        set => actions[key] = value;
    }

    public int Count => actions.Count;

    public void InitList()
    {
        if (actions == null)
            actions = new();
    }

    public void SetNTargets(int n)
    {
        nTargets = n;
    }

    internal void Add(CardAction act)
    {
        actions.Add(act);
    }
}

public class NonGod_Behaviour : Card_Behaviour
{
    protected new NonGod_Card_SO card_so;

    CardType cardType;
    [SerializeField]
    EventReference SoundClick;
    BoardElement target;
    Coroutine onSelectedRoutine;
    Coroutine actionRoutine;
    bool hasClickedTarget = false;
    bool cardIsReady = false;
    bool godBuffed = false;
    bool missedClick = false;

    public int neededLanes = 1;
    public List<ActionGroup> actions = new();
    public List<ActionGroup> godBuffActions = new();

    public CardType GetCardType => cardType;
    public new NonGod_Card_SO CardSO => card_so;

    private IEnumerator SelectingTargets()
    {
        CheckForGod();

        foreach (var actionGroup in actions)
        {
            foreach (var act in actionGroup.actions)
            {
                act.SetCamera();
                act.SetClickableTargets(controller.GetBoard(), true);
            }
            for (int i = 0; i < actionGroup.nTargets; i++)
            {
                hasClickedTarget = false;
                missedClick = false;
                yield return new WaitUntil(() => hasClickedTarget);
                foreach (var act in actionGroup.actions)
                {
                    act.AddTarget(target);
                }
                target = null;
            }
            foreach (var act in actionGroup.actions)
            {
                act.OnActionReady(controller.GetBoard());
                act.ResetCamera();
                act.SetClickableTargets(controller.GetBoard(), false);
            }
        }

        cardIsReady = true;
        controller.GetBoard().PlayCard(this);
        TurnController.shouldWaitForAnims = false;
    }
    private bool AllActionsReady()
    {
        return cardIsReady && onPlayerHand;
    }
    private void RemoveGodBuff()
    {
        if (godBuffed)
        {
            if (controller.GetBoard().playedGodCard)
                if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
                {
                    controller.GetBoard().playedGodCard.DeBuff(this);
                }
            godBuffed = false;

            foreach (var godAction in godBuffActions)
                actions.Remove(godAction);
        }
    }

    public void MissClick()
    {
        missedClick = true;
        CancelSelection();
    }
    public void BuffedByGod() { godBuffed = true; }
    public void Initialize(NonGod_Card_SO card, CardElements elements)
    {
        this.card_so = card;

        actions = new();
        godBuffActions = new();

        for (int i = 0; i < card.targetActions.Count; i++)
        {
            actions.Add(new(ActionsForTarget.numOfTargets));
            actions[i].InitList();

            for (int j = 0; j < card.targetActions[i].Count; j++)
            {
                var act = GetAction(card.targetActions[i][j]);
                act.SetBehaviour(this);
                actions[i].Add(act);

                CardActionData currAction = card.targetActions[i].targetActions[j];

                act.action_SFX = currAction.action_SFX;
                act.PlayOnPlacedOrTriggered_SFX = currAction.PlayOnPlacedOrTriggered_SFX;

                act._VFX = currAction._VFX;
            }
        }

        for (int i = 0; i < card.onGodBuff.Count; i++)
        {
            godBuffActions.Add(new(ActionsForTarget.numOfTargets));
            godBuffActions[i].InitList();

            for (int j = 0; j < card.onGodBuff[i].Count; j++)
            {
                var act = GetAction(card.onGodBuff[i][j]);
                act.SetBehaviour(this);
                godBuffActions[i].Add(act);
                act.action_SFX = card.onGodBuff[i].targetActions[j].action_SFX;
                act.PlayOnPlacedOrTriggered_SFX = card.onGodBuff[i].targetActions[j].PlayOnPlacedOrTriggered_SFX;
            }
        }

        this.cardType = card.type;
        this.elements = elements;
    }
    public void Buff(int value, bool isMult)
    {
        foreach (var target in actions)
        {
            foreach (var act in target.actions)
            {
                act.Buff(value, isMult);
            }
        }
        ChangeStrengh(actions[0][card_so.cardStrenghIndex].Strengh);
    }
    public void DeBuff(int value, bool isMult)
    {
        foreach (var target in actions)
        {
            foreach (var act in target.actions)
            {
                act.DeBuff(value, isMult);
            }
        }
        ChangeStrengh(actions[0][card_so.cardStrenghIndex].Strengh);
    }
    public void CheckForGod()
    {
        if (controller.GetBoard().playedGodCard)
            if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
            {
                controller.GetBoard().playedGodCard.Buff(this);

                foreach (var godAction in godBuffActions)
                    actions.Add(godAction);
            }
    }
    public void RemoveFromHand()
    {
        controller.Discard(this);
    }
    public BoardElement[] getActionTargets(int action)
    {
        return actions[action].actions[0].targets.ToArray();
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
        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                StartCoroutine(action.OnAction(board, this));
                if (action.PlayOnPlacedOrTriggered_SFX)
                {
                    SoundPlayer.PlaySound(action.action_SFX, gameObject);
                }
                yield return new WaitUntil(() => action.Ready());
            }
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.Reset(board);
            }
        }

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
        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.OnLanePlaced(controller.GetBoard());
            }
        }
        if (godBuffed)
        {
            foreach (var target in godBuffActions)
            {
                foreach (var action in target.actions)
                {
                    action.OnLanePlaced(controller.GetBoard());
                }
            }
        }

        controller.PlacedCard();

        missedClick = true;
    }
    public override void OnAction()
    {
        TurnController.shouldWaitForAnims = true;

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

        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.Reset(controller.GetBoard());
            }
        }

        foreach (var target in godBuffActions)
        {
            foreach (var action in target.actions)
            {
                action.Reset(controller.GetBoard());
            }
        }

        RemoveGodBuff();
    }
}
