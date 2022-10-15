using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardPathAnim : PathAnimatorController.pathAnimation
{
    new public UnityEvent<Card_SO> CompletionTrigger;
    public Card_SO cardSO;

    public CardPathAnim(Card_SO card)
    {
        this.CompletionTrigger = new UnityEvent<Card_SO>();
        this.cardSO = card;
    }

    public override void completionTrigger()
    {
        CompletionTrigger?.Invoke(cardSO);
        _Complete = true;
    }
}
