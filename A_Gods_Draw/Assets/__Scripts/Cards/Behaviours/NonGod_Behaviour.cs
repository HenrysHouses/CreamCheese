using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonGod_Behaviour : Card_Behaviour
{
    protected int strengh;
    protected Buff_Behaviour theCardCANThatBuffThis;
    protected Buff_Behaviour theCardThatBuffsThis;

    protected NonGod_Card current;

    public override void Initialize(Card_SO card)
    {
        this.card = card;
        current = card as NonGod_Card;
        strengh = current.baseStrengh;
    }

    public void CanBeBuffedBy(Buff_Behaviour buff_)
    {
        theCardCANThatBuffThis = buff_;
        Debug.Log(this + " can be buffed by " + buff_);
    }

    public override void OnClick()
    {
        if (manager.GetState() == TurnManager.State.PlayerTurn && !played && !manager.IsACardSelected())
        {
            manager.SelectedCard(this);
            played = true;
        }
    }

    public virtual void GetBuff(bool isMultiplier, int amount)
    {
        if (isMultiplier)
        {
            strengh *= amount;
        }
        else
        {
            strengh += amount;
        }
    }
}
