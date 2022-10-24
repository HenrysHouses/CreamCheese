using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NonGod_Behaviour : Card_Behaviour
{
    List<CardAction> actions;

    [SerializeField]
    EventReference SoundClick;

    protected int strengh;

    protected new NonGod_Card_SO card_so;
    public new NonGod_Card_SO CardSO => card_so;

    public void Initialize(NonGod_Card_SO card)
    {
        this.card_so = card;
        strengh = card_so.strengh;

        for (int i = 0; i < card.cardActions.Count; i++)
        {
            actions.Add(GetAction(card.cardActions[i], card.actionStrengh[i]));
        }

    }

    public void Buff(int value, bool isMult)
    {
        if (isMult)
        {
            strengh *= value;
        }
        else
        {
            strengh += value;
        }
        ChangeStrengh(strengh);
    }

    public void DeBuff(int value, bool isMult)
    {
        if (isMult)
        {
            strengh /= value;
        }
        else
        {
            strengh -= value;
        }
        ChangeStrengh(strengh);
    }

    public void CancelBuffs()
    {
        strengh = card_so.strengh;
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


    public void CheckForGod(God_Behaviour god)
    {
        if (card_so.correspondingGod == god.CardSO.godAction)
        {
            god.Buff(this);
        }
    }

    protected override void OnBeingSelected()
    {
        if (controller.GetBoard().playedCards.Count >= 4)
        {
            return;
        }
        controller.shouldWaitForAnims = true;
        StartCoroutine(SelectingTargets());
    }

    IEnumerator SelectingTargets()
    {
        foreach (CardAction action in actions)
        {
            action.SelectTargets(controller.GetBoard());
            yield return new WaitUntil(() => action.Ready());
        }
        controller.shouldWaitForAnims = false;

        if (card_so.type != CardType.Buff)
            controller.GetBoard().placeCardOnLane(this);
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
            action.OnPlay(board);
            yield return new WaitUntil(() => action.Ready());
        }

        controller.shouldWaitForAnims = false;
    }
}
