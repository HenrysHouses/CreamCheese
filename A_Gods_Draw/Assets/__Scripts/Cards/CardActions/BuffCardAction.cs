using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCardAction : CardAction
{
    bool multiplies;

    public BuffCardAction(int strengh, bool mult) : base(strengh, strengh) { multiplies = mult; neededLanes = 0;}

    void SpawnCoins(int mount, NonGod_Behaviour card)
    {
        for (int i = 0; i < mount; i++)
        {
            var aux = Object.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"), card.transform);
            aux.transform.localPosition = Vector3.back * 8 + Vector3.back * i;
        }
    }

    public override void SetClickableTargets(BoardStateController board, bool to = true)
    {
        board.SetClickable(0, to);
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        isReady = true;
    }
    public override void OnActionReady(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in targets)
        {
            card.Buff(strengh, multiplies);
            SpawnCoins(strengh, card);
        }
        ResetCamera();

        Object.Destroy(current.transform.parent.parent.gameObject);
        current.RemoveFromHand();
    }
    public override void OnLanePlaced(BoardStateController board)
    {
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
