// Written by Javier Villegas
using System.Collections;
using UnityEngine;
using FMODUnity;

public class BuffCardAction : CardAction
{
    bool multiplies;
    public EventReference cionDrop_SFX;

    public BuffCardAction(){ multiplies = false; neededLanes = 0;}

    void SpawnCoins(int amount, NonGod_Behaviour card)
    {
        for (int i = 0; i < amount; i++)
        {
            var aux = Object.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"), card.transform);
            aux.transform.localPosition = Vector3.back * 8 + Vector3.back * i;
        }
    }

    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    //     board.SetClickable(0, to);
    // }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        isReady = true;
    }
    public override void OnActionReady(BoardStateController board, NonGod_Behaviour source)
    {
        foreach (NonGod_Behaviour card in source.stats.Targets)
        {
            card.Buff(source.stats.strength, multiplies);
            SpawnCoins(source.stats.strength, card);
        }
    }
    public override void OnLanePlaced(BoardStateController board, NonGod_Behaviour source)
    {
        board.RemoveFromLane(currentCard);
        currentCard.transform.parent.parent.position += Vector3.down * 10;
        currentCard.RemoveFromHand();
    }

    protected override void UpdateNeededLanes(NonGod_Behaviour beh)
    {
        if (beh.neededLanes > 0)
            beh.neededLanes--;
    }
}
