/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

public class CardPathAnim : PathAnimatorController.pathAnimation
{
    public UnityEvent<Card_SO> OnCardCompletionTrigger;
    public UnityEvent<Card_SO> OnCardStartTrigger;
    public UnityEvent<EventReference, GameObject> OnAnimStartSound;
    public UnityEvent<EventReference, GameObject> OnAnimEndSound;
    public Card_SO cardSO;
    EventReference sound;
    GameObject soundTarget;

    public CardPathAnim(Card_SO card, EventReference soundEvent, GameObject SoundEmitterTarget)
    {
        OnCardCompletionTrigger = new UnityEvent<Card_SO>();
        OnCardStartTrigger = new UnityEvent<Card_SO>();
        OnAnimStartSound = new UnityEvent<EventReference, GameObject>();
        OnAnimEndSound = new UnityEvent<EventReference, GameObject>();
        cardSO = card;
        sound = soundEvent;
        soundTarget = SoundEmitterTarget;
    }

    public override void completionTrigger(string animationName)
    {
        OnAnimEndSound?.Invoke(sound, soundTarget);
        OnCardCompletionTrigger?.Invoke(cardSO);
        OnAnimCompletionTrigger?.Invoke();
        _Complete = true;
        // string[] n = animationName.Split('_');
        // Debug.Log("anim complete: " + n[n.Length-1]);
        Debug.Log("complete " + animationName);
    }

    public override void startTrigger()
    {
        OnAnimStartSound?.Invoke(sound, soundTarget);
        OnCardStartTrigger?.Invoke(cardSO);
        OnAnimStartTrigger?.Invoke();
        _Started = true;
    }
}
