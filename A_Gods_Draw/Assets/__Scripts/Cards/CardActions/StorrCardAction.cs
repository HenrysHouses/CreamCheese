// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class StorrCardAction : CardAction
{
    GameObject thing = null;
    public StorrCardAction(){ neededLanes = 2;}

    public override void OnLanePlaced(BoardStateController board)
    {
        for (int i = 0; i < cardStats.strength; i++)
        {
            thing = Object.Instantiate(Resources.Load<GameObject>("StorrThing"));
            board.placeThingOnLane(thing.GetComponent<BoardElement>());
        }
    }

    protected override void UpdateNeededLanes(NonGod_Behaviour beh)
    {
        beh.neededLanes += cardStats.strength;
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

    public override void ResetCamera()
    {
    }
    public override void SetCamera()
    {
    }
}