using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorrCardAction : CardAction
{
    GameObject thing = null;
    public StorrCardAction(int strengh) : base(strengh, strengh) { neededLanes = 2; }

    public override void OnLanePlaced(BoardStateController board)
    {
        thing = Object.Instantiate(Resources.Load<GameObject>("StorrThing"));
        board.placeThingOnLane(thing.GetComponent<BoardElement>());
    }

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;
        yield return new WaitUntil(() => true);
        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        Object.Destroy(thing);
        thing = null;
        isReady = false;
        board.SetClickable(3, false);
        ResetCamera();
    }
    public override void ResetCamera()
    {
    }
    public override void SetCamera()
    {
    }
}