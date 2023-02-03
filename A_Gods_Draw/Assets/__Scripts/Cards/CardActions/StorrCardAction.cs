// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class StorrCardAction : CardAction
{
    GameObject thing = null;
    public StorrCardAction(){ neededLanes = 2;}

    public override void OnLanePlaced(BoardStateController board, ActionCard_Behaviour source)
    {
        for (int i = 0; i < source.stats.strength; i++)
        {
            thing = Object.Instantiate(Resources.Load<GameObject>("StorrThing"));
            board.placeThingOnLane(thing.GetComponent<BoardElement>());
        }
    }

    protected override void UpdateNeededLanes(ActionCard_Behaviour source)
    {
        source.neededLanes += source.stats.strength;
    }

    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    // }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;
        //Object.Destroy(thing);
        yield return new WaitUntil(() => true);
        isReady = true;
    }
}