using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAction : Action
{
    public List<BoardElement> targets = new();

    protected NonGod_Behaviour current;

    protected int strengh;

    protected int neededLanes = 1;

    public int Strengh => strengh;

    public CardAction(int _min, int _max) : base(_min, _max) { strengh = _max; }

    public virtual void Buff(int amount, bool isMult)
    {
        if (isMult)
        {
            strengh *= amount;
        }
        else
        {
            strengh += amount;
        }
    }
    public virtual void DeBuff(int amount, bool isMult)
    {
        if (isMult)
        {
            strengh /= amount;
        }
        else
        {
            strengh -= amount;
        }
    }

    IEnumerator cor;

    public Animator camAnim = Camera.main.GetComponent<Animator>();

    public virtual void SetClickableTargets(BoardStateController board, bool to = true)
    {
        board.SetClickable(3, to);
    }

    public void SetBehaviour(NonGod_Behaviour beh)
    {
        current = beh;
        UpdateNeededLanes(beh);
    }

    protected virtual void UpdateNeededLanes(NonGod_Behaviour beh)
    {
    }

    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public virtual void OnLanePlaced(BoardStateController board)
    {

    }

    public abstract IEnumerator OnAction(BoardStateController board);

    public virtual void OnPlay(BoardStateController board) { }

    internal bool Ready()
    {
        return isReady;
    }

    public abstract void Reset(BoardStateController board);
    public virtual void OnActionReady(BoardStateController board) { }
    public abstract void ResetCamera();
    public abstract void SetCamera();

    internal void AddTarget(BoardElement target)
    {
        targets.Add(target);
    }

    public bool CanBePlaced(BoardStateController cont)
    {
        return cont.thingsInLane.Count + neededLanes <= 4;
    }
}
 