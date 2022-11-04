using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
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

    //Sounds

    protected bool onPlayerHand = false;

    // public void SetManager(TurnManager manager)
    // {
    //     this.manager = manager;
    // }
    public void SetController(TurnController cont)
    {
        onPlayerHand = true;
        controller = cont;
    }

    public void OnBeingClicked()
    {
        if (onPlayerHand)
        {
            SoundPlayer.Playsound(elements.OnClickSFX,gameObject);
            controller.SetSelectedCard(this);
            OnBeingSelected();
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

    protected CardAction GetAction(CardActionData card)
    {
        switch (card.actionEnum)
        {
            case CardActionEnum.Attack:
                return new AttackCardAction(card.actionStrength);
            case CardActionEnum.Defend:
                return new DefendCardAction(card.actionStrength);
            case CardActionEnum.Buff:
                return new BuffCardAction(card.actionStrength, false);
            case CardActionEnum.Instakill:
                return new InstakillCardAction(card.actionStrength);
            case CardActionEnum.Chained:
                return new ChainCardAction(card.actionStrength);
            case CardActionEnum.Storr:
                return new StorrCardAction(card.actionStrength);
            default:
                return null;
        }
    }

    public virtual void OnPlacedInLane()
    {
        
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

    public virtual void CancelSelection() { controller.SetSelectedCard(); }
    public virtual void Placed(bool placed = false)
    {
        GetComponent<BoxCollider>().enabled = true;
        transform.parent.GetComponent<BoxCollider>().enabled = false;
        onPlayerHand = placed;
    }
    public bool IsOnHand() { return onPlayerHand; }

    public abstract bool CardIsReady();
}