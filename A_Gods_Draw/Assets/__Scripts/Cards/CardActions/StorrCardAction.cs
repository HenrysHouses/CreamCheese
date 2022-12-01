using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorrCardAction : CardAction
{
    GameObject thing = null;
    public StorrCardAction(int strengh) : base(strengh, strengh) { neededLanes = 2; }

    public override void OnLanePlaced(BoardStateController board)
    {
        for (int i = 0; i < strengh; i++)
        {
            thing = Object.Instantiate(Resources.Load<GameObject>("StorrThing"));
            board.placeThingOnLane(thing.GetComponent<BoardElement>());
        }
    }

    protected override void UpdateNeededLanes(NonGod_Behaviour beh)
    {
        beh.neededLanes += strengh;
    }

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
    }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;
        //Object.Destroy(thing);
        yield return new WaitUntil(() => true);
        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
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