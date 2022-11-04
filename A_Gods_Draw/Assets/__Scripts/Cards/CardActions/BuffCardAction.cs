using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCardAction : CardAction
{
    bool multiplies;
    NonGod_Behaviour target;

    public BuffCardAction(int strengh, bool mult) : base(strengh, strengh) { multiplies = mult; }

    public override IEnumerator ChoosingTargets(BoardStateController board, float mult)
    {
        camAnim.SetBool("Up", true);

        isReady = false;

        board.SetClickable(0);

        yield return new WaitUntil(HasClickedNonGod);

        camAnim.SetBool("Up", false);


        board.SetClickable(0, false);

        int totalStrengh = max * (int)(mult + 0.1f);

        target.Buff(totalStrengh, multiplies);

        SpawnCoins(totalStrengh);

        current.RemoveFromHand();
        Object.Destroy(current.transform.parent.parent.gameObject);

        isReady = true;
    }

    void SpawnCoins(int mount)
    {
        for (int i = 0; i < mount; i++)
        {
            var aux = Object.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"), target.transform);
            aux.transform.localPosition = Vector3.back * 8 + Vector3.back * i;
        }
    }

    bool HasClickedNonGod()
    {
        BoardElement element = TurnController.PlayerClick();
        NonGod_Behaviour clickedCard = element as NonGod_Behaviour;
        if (clickedCard)
        {
            Debug.Log("To buff: " + clickedCard.CardSO.cardName);
            target = clickedCard;
            return true;
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;

        yield return new WaitUntil(() => true);

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        target = null;
        isReady = false;
        board.SetClickable(0, false);
        ResetCamera();
    }
    public override void ResetCamera()
    {
        camAnim.SetBool("Up", false);
    }
}
