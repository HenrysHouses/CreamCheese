using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAction : Action
{
    public CardAction(int _min, int _max) : base(_min, _max) { }

    IEnumerator cor;

    public void GetCoroutines(out IEnumerator enumerator1)
    {
        enumerator1 = cor;
        cor = null;
    }
    public override void Execute(BoardStateController board, int strengh) { }

    public abstract IEnumerator ChoosingTargets(BoardStateController board);

    public void Act(BoardStateController board)
    {
        isReady = false;
        cor = OnAction(board);
    }

    public abstract IEnumerator OnAction(BoardStateController board);

    public virtual void OnPlay(BoardStateController board) { }

    internal bool Ready()
    {
        return isReady;
    }
}
 