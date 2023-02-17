// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class StorrCardAction : CardAction
{
    GameObject thing = null;

    public override void CardPlaced(BoardStateController board, ActionCard_Behaviour source)
    {
        for (int i = 0; i < source.stats.strength; i++)
        {
            thing = Object.Instantiate(Resources.Load<GameObject>("StorrThing"));
            board.placeThingOnLane(thing.GetComponent<BoardElement>());
        }
    }

    protected override void UpdateNeededLanes(ActionCard_Behaviour source)
    {
        source.neededLanes += 1;
    }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;
        //Object.Destroy(thing);
        yield return new WaitUntil(() => true);
        isReady = true;
    }
}