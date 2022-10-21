using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public abstract class NonGod_Behaviour : Card_Behaviour
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
        card_so = card as NonGod_Card_SO;
        strengh = card_so.strengh;

        foreach (CardActionEnum actionEnum in card.cardActions.Keys)
        {
            actions.Add(GetAction(actionEnum, card.cardActions[actionEnum]));
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
    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in board.playedCards)
        {
            if (card.CardSO.correspondingGod == card_so.godAction)
            {
                card.Buff(card_so.strengh, true);
            }
        }

        foreach (CardAction action in actions)
        {
            action.OnPlay(board);
            yield return new WaitUntil(() => true /* action.IsReady() */);
        }


        controller.shouldWaitForAnims = false;
    }
}
