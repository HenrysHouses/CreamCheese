using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static TurnManager;

public abstract class Card_Behaviour : MonoBehaviour
{
    IEnumerator cor;

    protected Card_SO card_so;
    public Card_SO CardSO => card_so;
    public string Name => card_so.cardName;
    // protected TurnManager manager;

    public readonly bool isReady = false;

    protected TurnController controller;

    protected CardElements elements;


    // public void SetManager(TurnManager manager)
    // {
    //     this.manager = manager;
    // }

    public virtual void OnClick()
    {
        // if(!manager)
        // {
        //     // Debug.LogWarning("A card does not have reference to the turnManager");
        //     return;
        // }

        // if (manager.CurrentlySelectedCard() == this)
        // {
        //     manager.CancelSelection();
        //     return;
        // }
        // if (!manager.CurrentlySelectedCard() || this != manager.CurrentlySelectedCard())
        // {
        //     manager.CancelSelection();
        //     manager.SelectCard(this);
        //     GetComponentInParent<Card_ClickGlowing>().ShowBorder();
        // }
    }
    // public void setCardSO(Card_SO Stats) { card = Stats; }

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

    protected abstract IEnumerator Play(BoardStateController board)
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
                return new BuffCardAction(strengh);
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
    public virtual void OnAction() { }
}