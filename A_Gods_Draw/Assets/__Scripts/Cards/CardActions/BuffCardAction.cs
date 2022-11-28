using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BuffCardAction : CardAction
{
    bool multiplies;
    public EventReference cionDrop_SFX;

    public BuffCardAction(int strengh, bool mult) : base(strengh, strengh) { multiplies = mult; neededLanes = 0;}

    void SpawnCoins(int mount, NonGod_Behaviour card)
    {
        for (int i = 0; i < mount; i++)
        {
            var aux = Object.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"), card.transform);
            aux.transform.localPosition = Vector3.back * 8 + Vector3.back * i;
          //  SoundPlayer.PlaySound(cionDrop_SFX,aux);
            Debug.LogWarning("Hvor er lyden? :Pleading:");
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
    }
    public override void OnLanePlaced(BoardStateController board)
    {
        board.RemoveFromLane(current);
        Object.Destroy(current.transform.parent.parent.gameObject);
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
