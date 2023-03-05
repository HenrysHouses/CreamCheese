// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class OfferingCardAction : CardAction
{
    GameObject thing = null;

    public override void CardPlaced(BoardStateController board, ActionCard_Behaviour source)
    {
        thing = Object.Instantiate(Resources.Load<GameObject>("Offering_PRE"));
        board.placeThingOnLane(thing.GetComponent<BoardElement>());
    }

    protected override void UpdateNeededLanes(ActionCard_Behaviour source)
    {
        source.neededLanes += 1;
    }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = true;
        yield break;
    }
}