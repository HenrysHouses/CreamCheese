using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonGod_Behaviour : Card_Behaviour
{
    protected int strengh;

    Buff_Behaviour possibleBuff;

    public override void Initialize(Card_SO card)
    {
        this.card = card;
        strengh = (card as NonGod_Card).baseStrengh;
    }

    public void CanBeBuffedBy(Buff_Behaviour buff_)
    {
        possibleBuff = buff_;
        Debug.Log(this + " can be buffed by " + buff_);
    }

    protected override void OnMouseDown()
    {
        if (possibleBuff && possibleBuff != this)
        {
            strengh = possibleBuff.GetBuffedStat(strengh);
            Debug.Log(possibleBuff + " is going to buff this to " + strengh);
        }
        else if (manager.GetState() == TurnManager.State.PlayerTurn)
        {
            manager.SelectedCard(this);
        }
    }
    public virtual void GetGodBuff(bool isMultiplier, float amount) { }
    public virtual void GetBuff(bool isMultiplier, short amount) { }
}
