using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card_Behaviour : MonoBehaviour
{
    Card_SO card;

    public void Initialize(Card_SO card)
    {
        this.card = card;
    }

    public Card_SO GetCardSO() { return card; }
    // public void setCardSO(Card_SO Stats) { card = Stats; }

    public virtual void OnPlay() { }
    public virtual void OnAction() { }
}
