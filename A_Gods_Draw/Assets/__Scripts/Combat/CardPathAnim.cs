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
    public UnityEvent<GodDialogueTrigger, UnityEngine.Object> OnCardDrawDialogue;
    public UnityEvent<EventReference, GameObject> OnAnimStartSound;
    public UnityEvent<EventReference, GameObject> OnAnimEndSound;
    public Card_SO cardSO;
    EventReference sound;
    GameObject soundTarget;
    GodDialogueTrigger _dialogueTrigger;

    public CardPathAnim(Card_SO card, EventReference soundEvent, GameObject SoundEmitterTarget, GodDialogueTrigger dialogueTrigger)
    {
        OnCardCompletionTrigger = new UnityEvent<Card_SO>();
        OnCardStartTrigger = new UnityEvent<Card_SO>();
        OnCardDrawDialogue = new UnityEvent<GodDialogueTrigger, Object>();
        OnAnimStartSound = new UnityEvent<EventReference, GameObject>();
        OnAnimEndSound = new UnityEvent<EventReference, GameObject>();
        OnAnimCompletionTrigger = new UnityEvent();
        OnAnimStartTrigger = new UnityEvent();

        cardSO = card;
        sound = soundEvent;
        soundTarget = SoundEmitterTarget;
        _dialogueTrigger = dialogueTrigger;
    }

    public override void completionTrigger(string animationName)
    {
        OnCardDrawDialogue?.Invoke(_dialogueTrigger, cardSO);
        OnAnimEndSound?.Invoke(sound, soundTarget);
        OnCardCompletionTrigger?.Invoke(cardSO);
        OnAnimCompletionTrigger?.Invoke();
        _Complete = true;
        //Debug.Log("complete " + animationName);
    }

    public override void startTrigger()
    {
        OnAnimStartSound?.Invoke(sound, soundTarget);
        OnCardStartTrigger?.Invoke(cardSO);
        OnAnimStartTrigger?.Invoke();
        _Started = true;
    }
}
