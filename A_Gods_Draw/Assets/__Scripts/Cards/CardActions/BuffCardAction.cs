// Written by Javier Villegas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class BuffCardAction : CardAction
{
    bool multiplies;
    public EventReference cionDrop_SFX;
    List<GameObject> SpawnedCoins = new List<GameObject>();
    public BuffCardAction(){ multiplies = false; neededLanes = 0;}
    
    void SpawnCoins(int amount, ActionCard_Behaviour card)
    {
        for (int i = 0; i < amount; i++)
        {
            Debug.Log("spawning coin");
            GameObject spawn  = GameObject.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"));
            SpawnedCoins.Add(spawn);
            spawn.transform.SetParent(card.RootTransform, false);
            Vector3 offsetPos = spawn.transform.localPosition;
            offsetPos.x += Random.Range(-0.01f, 0.01f);
            offsetPos.y += 10;
            offsetPos.z += Random.Range(-0.01f, 0.01f);
            spawn.transform.localPosition = offsetPos;
        }
    }

    public override void Reset(BoardStateController board, Card_Behaviour source)
    {
        foreach (var item in SpawnedCoins)
        {
            GameObject.Destroy(item);
        }
        base.Reset(board, source);
    }

    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    //     board.SetClickable(0, to);
    // }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        isReady = true;
    }
    public override void OnActionReady(BoardStateController board, ActionCard_Behaviour source)
    {
        foreach (ActionCard_Behaviour card in source.AllTargets)
        {
            card.Buff(source.stats.strength, multiplies);
            SpawnCoins(source.stats.strength, card);
        }
    }
    public override void OnLanePlaced(BoardStateController board, ActionCard_Behaviour source)
    {
        board.RemoveFromLane(currentCard);
        currentCard.transform.parent.parent.position += Vector3.down * 10;
        currentCard.RemoveFromHand();
    }

    protected override void UpdateNeededLanes(ActionCard_Behaviour beh)
    {
        if (beh.neededLanes > 0)
            beh.neededLanes--;
    }
}
