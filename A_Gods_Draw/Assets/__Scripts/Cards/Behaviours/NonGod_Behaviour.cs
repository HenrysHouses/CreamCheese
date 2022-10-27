using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NonGod_Behaviour : Card_Behaviour
{
    List<CardAction> actions = new List<CardAction>();

    [SerializeField]
    EventReference SoundClick;

    protected int strengh;

    protected new NonGod_Card_SO card_so;
    public new NonGod_Card_SO CardSO => card_so;

    IEnumerator onSelectedRoutine;
    IEnumerator actionRoutine;
    public void Initialize(NonGod_Card_SO card, CardElements elements)
    {
        this.card_so = card;
        strengh = card_so.strengh;

        for (int i = 0; i < card.cardActions.Count; i++)
        {
            actions.Add(GetAction(card.cardActions[i].actionEnum, card.cardActions[i].actionStrength));
        }

        this.elements = elements;
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
            if (controller.GetBoard().playedCards.Count >= 4 && card_so.type != CardType.Buff)
            {
                return;
            }
            controller.shouldWaitForAnims = true;
            onSelectedRoutine = SelectingTargets();
            StartCoroutine(onSelectedRoutine);
        }
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
            StartCoroutine(action.OnAction(board, strengh));
            yield return new WaitUntil(() => action.Ready());
        }

        yield return new WaitForSeconds(0.2f);

        Destroy(transform.parent.parent.gameObject);
        controller.shouldWaitForAnims = false;
    }

    public int GetStrengh() { return strengh; }

    public override void CancelSelection()
    {
        base.CancelSelection();
        if (onSelectedRoutine != null)
            StopCoroutine(onSelectedRoutine);
        if (actionRoutine != null)
            StopCoroutine(actionRoutine);
        onSelectedRoutine = null;
        actionRoutine = null;

        foreach (CardAction action in actions)
        {
        }
    }

    public override bool CardIsReady()
    {
        return AllActionsReady();
    }
}
