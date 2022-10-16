/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardPathAnim : PathAnimatorController.pathAnimation
{
    public UnityEvent<Card_SO> OnCardCompletionTrigger;
    public UnityEvent<Card_SO> OnCardStartTrigger;
    public Card_SO cardSO;

    public CardPathAnim(Card_SO card)
    {
        OnCardCompletionTrigger = new UnityEvent<Card_SO>();
        OnCardStartTrigger = new UnityEvent<Card_SO>();
        cardSO = card;
    }

    public override void completionTrigger(string animationName)
    {
        OnCardCompletionTrigger?.Invoke(cardSO);
        OnAnimCompletionTrigger?.Invoke();
        _Complete = true;
        // string[] n = animationName.Split('_');
        // Debug.Log("anim complete: " + n[n.Length-1]);
        Debug.Log("complete " + animationName);
    }

    public override void startTrigger()
    {
        OnCardStartTrigger?.Invoke(cardSO);
        OnAnimStartTrigger?.Invoke();
        _Started = true;
    }
}
