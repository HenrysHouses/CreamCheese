using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;

public abstract class Card_Behaviour : MonoBehaviour
{
    protected Card_SO card;
    protected TurnManager manager;

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
        if (!manager.CurrentlySelectedCard() || this != manager.CurrentlySelectedCard())
        {
            manager.CancelSelection();
            manager.SelectCard(this);
            GetComponentInParent<Card_ClickGlowing>().ShowBorder();
        }
    }

    public Card_SO GetCardSO() { return card; }
    // public void setCardSO(Card_SO Stats) { card = Stats; }

    public virtual IEnumerator OnPlay(BoardState board)
    {
        yield return new WaitUntil(ReadyToBePlaced);
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
        manager.FinishedPlay(this);
    }

    public void DeSelected()
    {
        StopAllCoroutines();
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
    }

    public virtual void OnAction() { }

    internal TurnManager GetManager()
    {
        return manager;
    }

    public bool IsThisSelected()
    {
        if(manager == null)
        {
            return false;
        }
        return manager.CurrentlySelectedCard() == this;
    }

    protected virtual bool ReadyToBePlaced()
    {
        return true;
    }
}
