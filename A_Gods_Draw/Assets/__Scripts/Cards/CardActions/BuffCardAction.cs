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

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
        board.SetClickable(0, to);
    }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        isReady = true;
    }
    public override void OnActionReady(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in targets)
        {
            card.Buff(cardStats.strength, multiplies);
            SpawnCoins(cardStats.strength, card);
        }
        ResetCamera();
    }
    public override void OnLanePlaced(BoardStateController board)
    {
        board.RemoveFromLane(current);
        current.transform.parent.parent.position += Vector3.down * 10;
        current.RemoveFromHand();
    }

    protected override void UpdateNeededLanes(NonGod_Behaviour beh)
    {
        if (beh.neededLanes > 0)
            beh.neededLanes--;
    }

    public override void Reset(BoardStateController board)
    {
        targets.Clear();
        isReady = false;
        board.SetClickable(0, false);
        ResetCamera();
    }
    public override void ResetCamera()
    {
        camAnim.SetBool("Up", false);
    }
    public override void SetCamera()
    {
        camAnim.SetBool("Up", true);
    }
}
