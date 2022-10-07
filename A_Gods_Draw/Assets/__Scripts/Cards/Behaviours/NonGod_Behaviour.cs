using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public abstract class NonGod_Behaviour : Card_Behaviour
{
    [SerializeField]
    EventReference SoundClick;

    protected int strengh;
    protected Buff_Behaviour theCardCANThatBuffThis;
    protected Buff_Behaviour theCardThatBuffsThis;

    protected NonGod_Card card_NonGod;

    public NonGod_Card GetNonGod() { return card_NonGod; }

    public override void Initialize(Card_SO card)
    {
        this.card_abs = card;
        card_NonGod = card as NonGod_Card;
        strengh = card_NonGod.baseStrengh;
    }

    public void CanBeBuffedBy(Buff_Behaviour buff_)
    {
        theCardCANThatBuffThis = buff_;
        //Debug.Log(this + " can be buffed by " + buff_);
    }

    //public override void OnClick()
    //{
    //    if (manager.CurrentlySelectedCard() == this)
    //    {
    //        manager.CancelSelection();

    //        //Debug.Log("you clicked me, and im not being played");
    //        return;
    //    }
    //    if (manager.GetState() == TurnManager.State.PlayerTurn && !manager.CurrentlySelectedCard())
    //    {
    //        manager.SelectCard(this);
    //        //Debug.Log("you clicked me, and im being played");
    //    }

    //    //Debug.Log(manager.CurrentlySelectedCard().gameObject);
    //}

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
        if (card_NonGod.correspondingGod == god.GetName())
        {
            god.Buff(this);
        }
    }

    internal virtual void PlacedNextToThis(NonGod_Behaviour card)
    {

    }

    public override void LatePlayed(BoardState board)
    {
        base.LatePlayed(board);

        if (board.currentGod)
            CheckForGod(board.currentGod);

        if (board.lane.Count > 0)
        {
            board.lane[^1].PlacedNextToThis(this);
        }
    }
}
