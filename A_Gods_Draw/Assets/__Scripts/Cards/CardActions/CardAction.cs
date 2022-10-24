using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAction : Action
{
    public CardAction(int _min, int _max) : base(_min, _max) { }

    IEnumerator cor;

    public void CancelEverything()
    {
        cor.Reset();
    }
    public override void Execute(BoardStateController board, int strengh) { }

    public void SelectTargets(BoardStateController board)
    {
        isReady = false;
        cor = ChoosingTargets(board);
    }

    protected abstract IEnumerator ChoosingTargets(BoardStateController board);

    public void Act(BoardStateController board)
    {
        isReady = false;
        cor = OnAction(board);
    }

    protected abstract IEnumerator OnAction(BoardStateController board);

    public virtual void OnPlay(BoardStateController board) { }

    internal bool Ready()
    {
        return isReady;
    }
}
 