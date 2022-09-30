using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonGod_Behaviour : Card_Behaviour
{
    protected int strengh;
    protected Buff_Behaviour theCardCANThatBuffThis;
    protected Buff_Behaviour theCardThatBuffsThis;

    protected NonGod_Card current;

    public NonGod_Card GetNonGod() { return current; }

    public override void Initialize(Card_SO card)
    {
        this.card = card;
        current = card as NonGod_Card;
        strengh = current.baseStrengh;
    }

    public void CanBeBuffedBy(Buff_Behaviour buff_)
    {
        theCardCANThatBuffThis = buff_;
        //Debug.Log(this + " can be buffed by " + buff_);
    }

    public override void OnClick()
    {
        if (manager.CurrentlySelectedCard() == this)
        {
            manager.CancelSelection();
            played = false;
            return;
        }
        if (manager.GetState() == TurnManager.State.PlayerTurn && !played && !manager.CurrentlySelectedCard())
        {
            manager.SelectCard(this);
            played = true;
        }
    }

    public virtual void GetBuff(bool isMultiplier, float amount)
    {
        if (isMultiplier)
        {
            strengh = (int)(strengh * amount);
        }
        else
        {
            strengh = (int)(strengh + amount);
        }

        GetComponent<Card_Loader>().ChangeStrengh(strengh);
    }

    public void CheckForGod(God_Behaviour god)
    {
        if (current.correspondingGod == god.GetName())
        {
            god.Buff(this);
        }
    }
}
