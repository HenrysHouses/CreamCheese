using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;

public abstract class Card_Behaviour : MonoBehaviour
{
    IEnumerator cor;

    protected Card_SO card_abs;
    protected TurnManager manager;

    public readonly bool isReady = false;


    public virtual void Initialize(Card_SO card)
    {
        this.card_abs = card;
    }

    public void SetManager(TurnManager manager)
    {
        this.manager = manager;
    }

    public virtual void OnClick()
    {
        if(!manager)
        {
            // Debug.LogWarning("A card does not have reference to the turnManager");
            return;
        }

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

    public Card_SO GetCardSO() { return card_abs; }
    // public void setCardSO(Card_SO Stats) { card = Stats; }

    public void DeSelected()
    {
        StopCoroutine(cor);
        cor = null;
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
    }

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

    public void OnPlay(BoardStateController board)
    {
        cor = Play(board);
        StartCoroutine(cor);
        transform.localScale = new Vector3(0.25f,0.20f,0.20f);
    }

    protected virtual IEnumerator Play(BoardStateController board)
    {
        yield return new WaitUntil(ReadyToBePlaced);
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
        GetComponentInParent<BoxCollider>().enabled = false;
        LatePlayed(board);
        manager.FinishedPlay(this);

        
        if(this is Attack_Behaviour attack_)
            manager.OnDeSelectedAttackCard?.Invoke();
    }
    public virtual void LatePlayed(BoardStateController board) { }
    public virtual void OnAction() { }
}