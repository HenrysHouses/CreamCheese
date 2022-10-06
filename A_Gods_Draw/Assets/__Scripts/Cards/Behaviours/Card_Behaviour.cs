using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;

public abstract class Card_Behaviour : MonoBehaviour
{
    protected Card_SO card;
    protected TurnManager manager;

    protected bool played = false;

    public readonly bool isReady = false;

    public virtual void Initialize(Card_SO card)
    {
        this.card = card;
    }

    public void SetManager(TurnManager manager)
    {
        this.manager = manager;
    }

    public virtual void OnClick()
    {
        if (manager.CurrentlySelectedCard() == this)
        {
            manager.CancelSelection();
            return;
        }
        if (!played && !manager.CurrentlySelectedCard())
        {
            manager.SelectCard(this);
            played = true;
        }
    }

    public Card_SO GetCardSO() { return card; }
    // public void setCardSO(Card_SO Stats) { card = Stats; }

    public virtual IEnumerator OnPlay(BoardState board)
    {
        yield return new WaitUntil(() => { return true; });
        manager.FinishedPlay(this);
    }
    public virtual void OnAction() { }

    internal TurnManager GetManager()
    {
        return manager;
    }

    public bool IsThisSelected()
    {
        return manager.CurrentlySelectedCard() == this;
    }
}
