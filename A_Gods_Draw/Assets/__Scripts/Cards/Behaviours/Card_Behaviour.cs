using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static TurnManager;

public abstract class Card_Behaviour : BoardElement
{
    IEnumerator cor;

    protected Card_SO card_so;
    public Card_SO CardSO => card_so;
    public string Name => card_so.cardName;
    // protected TurnManager manager;

    public readonly bool isReady = false;

    protected TurnController controller;

    protected CardElements elements;

    bool onPlayerHand;

    // public void SetManager(TurnManager manager)
    // {
    //     this.manager = manager;
    // }

    public override bool OnClick()
    {
        if (onPlayerHand)
        {
            if (controller.SelectedCard != this)
            {
                controller.SetSelectedCard(this);
                OnBeingSelected();
            }
            return false;
        }
        else
        {
            return base.OnClick();
        }
    }

    protected abstract void OnBeingSelected();

    public void DeSelected()
    {
        StopCoroutine(cor);
        cor = null;
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
    }

    // internal TurnManager GetManager()
    // {
    //     return manager;
    // }

    // public bool IsThisSelected()
    // {
    //     if(manager == null)
    //     {
    //         return false;
    //     }
    //     return manager.CurrentlySelectedCard() == this;
    // }

    protected virtual bool ReadyToBePlaced()
    {
        return true;
    }

    public void OnPlay(BoardStateController board)
    {
        controller.shouldWaitForAnims = true;
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
        // manager.FinishedPlay(this);

        
        // if(this is Attack_Behaviour attack_)
        //     manager.OnDeSelectedAttackCard?.Invoke();
    }

    protected CardAction GetAction(CardActionEnum card, int strengh)
    {
        switch (card)
        {
            case CardActionEnum.Attack:
                return new AttackCardAction(strengh);
            case CardActionEnum.Defend:
                return new DefendCardAction(strengh);
            case CardActionEnum.Buff:
                return new BuffCardAction(strengh, false);
            case CardActionEnum.Instakill:
                return new InstakillCardAction(strengh);
            default:
                return null;
        }
    }

    protected GodCardAction GetAction(GodActionEnum card)
    {
        return card switch
        {
            GodActionEnum.Tyr => new TyrActions(),
            _ => null,
        };
    }

    public void ChangeStrengh(int newValue)
    {
        elements.strength.text = newValue.ToString();
    }

    public virtual void LatePlayed(BoardStateController board) { }
    public abstract void OnAction();

    internal void CancelSelection()
    {
        //Actions cancel everythign;
    }
}