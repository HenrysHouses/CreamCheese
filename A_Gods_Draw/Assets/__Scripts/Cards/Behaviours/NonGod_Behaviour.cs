using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class NonGod_Behaviour : Card_Behaviour
{
    List<CardAction> actions = new List<CardAction>();
    public int TargetedActions => actions.Count;

    [SerializeField]
    EventReference SoundClick;

    protected new NonGod_Card_SO card_so;
    public new NonGod_Card_SO CardSO => card_so;

    IEnumerator onSelectedRoutine;
    IEnumerator actionRoutine;
    public void Initialize(NonGod_Card_SO card, CardElements elements)
    {
        this.card_so = card;

        for (int i = 0; i < card.cardActions.Count; i++)
        {
            actions.Add(GetAction(card.cardActions[i]));
            actions[i].SetBehaviour(this);
        }

        this.elements = elements;
    }

    public void Buff(int value, bool isMult)
    {
        foreach (CardAction act in actions)
        {
            act.Buff(value, isMult);
        }
        ChangeStrengh(actions[card_so.cardStrenghIndex].Strengh);
    }

    public void DeBuff(int value, bool isMult)
    {
        foreach (CardAction act in actions)
        {
            act.DeBuff(value, isMult);
        }
        ChangeStrengh(actions[card_so.cardStrenghIndex].Strengh);
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
        return actions[action].target;
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
            controller.shouldWaitForAnims = true;
            onSelectedRoutine = SelectingTargets();
            StartCoroutine(onSelectedRoutine);
        }
    }

    public void RemoveFromHand()
    {
        controller.Discard(this);
    }

    IEnumerator SelectingTargets()
    {
        float mult = 1f;
        if (controller.GetBoard().playedGodCard)
            if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
            {
                mult = controller.GetBoard().playedGodCard.GetStrengh();
            }
        foreach (CardAction action in actions)
        {
            actionRoutine = action.ChoosingTargets(controller.GetBoard(), mult);
            StartCoroutine(actionRoutine);
            yield return new WaitUntil(() => action.Ready());
        }
        CheckForGod();
        controller.shouldWaitForAnims = false;
    }

    bool AllActionsReady()
    {
        bool aux = true;
        for (int i = 0; i < actions.Count && aux; i++)
        {
            aux = actions[i].Ready();
        }
        if (aux)
        {
            Debug.Log("ready");
        }
        return aux && onPlayerHand;
    }

    public override void OnAction()
    {
        controller.shouldWaitForAnims = true;
        StartCoroutine(Play(controller.GetBoard()));
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (CardAction action in actions)
        {
            StartCoroutine(action.OnAction(board));
            yield return new WaitUntil(() => action.Ready());
        }

        yield return new WaitForSeconds(0.2f);

        foreach (CardAction action in actions)
        {
            action.Reset(board);
        }

        controller.Discard(this);
        controller.shouldWaitForAnims = false;
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

            foreach (CardAction act in actions)
                act.ResetCamera();

            return true;
        }

        return false;
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
        foreach (CardAction action in actions)
        {
            action.OnLanePlaced(controller.GetBoard());
        }
        missedClick = true;
    }
}
