using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

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
    public List<ActionGroup> actions = new();
    public int TargetedActions()
    {
        int aux = 0;
        foreach (var group in actions)
        {
            aux++;
        }
        return aux;
    }
    CardType cardType;
    public CardType GetCardType => cardType;
    [SerializeField]
    EventReference SoundClick;

    bool cardIsReady = false;

    protected new NonGod_Card_SO card_so;
    public new NonGod_Card_SO CardSO => card_so;

    IEnumerator onSelectedRoutine;
    IEnumerator actionRoutine;

    public void Initialize(NonGod_Card_SO card, CardElements elements)
    {
        this.card_so = card;

        actions = new();

        for (int i = 0; i < card.targetActions.Count; i++)
        {
            actions.Add(new(card.targetActions[i].numOfTargets));
            actions[i].InitList();

            for (int j = 0; j < card.targetActions[i].Count; j++)
            {
                var act = GetAction(card.targetActions[i][j]);
                act.SetBehaviour(this);
                actions[i].Add(act);
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

    //public override void OnClick()
    //{
    //    if (manager.CurrentlySelectedCard() == this)
    //    {
    //        manager.CancelSelection();

    //        //Debug.Log("you clicked me, and im not being played");
    //        return;
    //    }
    //    if (manager.GetState() == TurnManager.State.PlayerTurn && !manager.CurrentlySelectedCard())
    //    {
    //        manager.SelectCard(this);
    //        //Debug.Log("you clicked me, and im being played");
    //    }

    //    //Debug.Log(manager.CurrentlySelectedCard().gameObject);
    //}

    public IMonster getActionTarget(int action)
    {
        int aux = action;
        foreach (var actionsInTarget in actions)
        {
            aux -= (actionsInTarget.Count - 1);
            if (aux <= 0)
            {
                return (actionsInTarget[actionsInTarget.Count - 1 + aux].targets[0] as IMonster);
            }
        }
        return null;
    }


    public void CheckForGod()
    {
        if (controller.GetBoard().playedGodCard)
            if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
            {
                controller.GetBoard().playedGodCard.Buff(this);
            }
    }

    protected override void OnBeingSelected()
    {
        if (onSelectedRoutine == null)
        {
            if (controller.GetBoard().thingsInLane.Count >= 4 && card_so.type != CardType.Buff)
            {
                return;
            }
            TurnController.shouldWaitForAnims = true;
            onSelectedRoutine = SelectingTargets();
            StartCoroutine(onSelectedRoutine);
        }
    }

    public void RemoveFromHand()
    {
        controller.Discard(this);
    }

    BoardElement target;

    IEnumerator SelectingTargets()
    {
        //yield return new WaitForSeconds(0.2f);

        CheckForGod();

        foreach (var actionGroup in actions)
        {
            foreach (var act in actionGroup.actions)
            {
                if (!act.CanBePlaced(controller.GetBoard()))
                {
                    DeSelected();
                    ForceCancelSelection();
                    StopAllCoroutines();
                    yield return null;
                }

                act.SetCamera();
                act.SetClickableTargets(controller.GetBoard(), true);
            }
            for (int i = 0; i < actionGroup.nTargets; i++)
            {
                yield return new WaitUntil(HasClickedTarget);
                foreach (var act in actionGroup.actions)
                {
                    act.AddTarget(target);
                }
            }
            foreach (var act in actionGroup.actions)
            {
                act.OnActionReady(controller.GetBoard());
                act.ResetCamera();
                act.SetClickableTargets(controller.GetBoard(), false);
            }
        }

        cardIsReady = true;
        TurnController.shouldWaitForAnims = false;
    }

    private void ForceCancelSelection()
    {
        base.CancelSelection();
        if (onSelectedRoutine != null)
            StopCoroutine(onSelectedRoutine);
        if (actionRoutine != null)
            StopCoroutine(actionRoutine);
        onSelectedRoutine = null;
        actionRoutine = null;

        RemoveGodBuff();

        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.ResetCamera();
            }
        }
    }

    bool hasClickedThisFrame = false;

    bool HasClickedTarget()
    {
        if (Input.GetMouseButtonDown(0) && !hasClickedThisFrame)
        {
            hasClickedThisFrame = true;
            BoardElement element = TurnController.PlayerClick();
            if (element)
            {
                Debug.Log(element);
                target = element;
                return true;
            }
            MissClick();
        }
        hasClickedThisFrame = false;
        return false;
    }

    bool AllActionsReady()
    {
        return cardIsReady && onPlayerHand;
    }

    public override void OnAction()
    {
        TurnController.shouldWaitForAnims = true;
        StartCoroutine(Play(controller.GetBoard()));
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                StartCoroutine(action.OnAction(board));
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
        TurnController.shouldWaitForAnims = false;
    }

    public override bool CancelSelection()
    {
        if (this == null)
            return true;

        if (HasMissedClick())
        {
            base.CancelSelection();
            if (onSelectedRoutine != null)
                StopCoroutine(onSelectedRoutine);
            if (actionRoutine != null)
                StopCoroutine(actionRoutine);
            onSelectedRoutine = null;
            actionRoutine = null;

            RemoveGodBuff();

            foreach (var target in actions)
            {
                foreach (var action in target.actions)
                {
                    action.ResetCamera();
                }
            }

            return true;
        }

        return false;
    }

    private void RemoveGodBuff()
    {
        if (controller.GetBoard().playedGodCard)
            if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
            {
                controller.GetBoard().playedGodCard.DeBuff(this);
            }
    }

    bool missedClick = false;
    private bool HasMissedClick()
    {
        if (missedClick)
        {
            missedClick = false;
            return true;
        }
        return false;
    }
    public void MissClick()
    {
        missedClick = true;
    }

    public override bool CardIsReady()
    {
        return AllActionsReady();
    }
    public override void OnPlacedInLane()
    {
        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.OnLanePlaced(controller.GetBoard());
            }
        }

        missedClick = true;
    }
}
