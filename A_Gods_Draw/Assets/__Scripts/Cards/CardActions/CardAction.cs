using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAction : Action
{
    protected NonGod_Behaviour current;

    protected int strengh;

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

    public void SetBehaviour(NonGod_Behaviour beh)
    {
        current = beh;
    }

    public override void Execute(BoardStateController board, int strengh) { }

    public abstract IEnumerator ChoosingTargets(BoardStateController board, float mult);

    public virtual void OnLanePlaced(BoardStateController board) { }

    public abstract IEnumerator OnAction(BoardStateController board);

    public virtual void OnPlay(BoardStateController board) { }

    internal bool Ready()
    {
        return isReady;
    }

    public abstract void Reset(BoardStateController board);
    public abstract void ResetCamera();
}
 