/* 
 * Written by 
 * Henrik
*/

using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

/// <summary>
/// Card animation data container for animation requests
/// </summary>
public class CardPathAnim : PathAnimatorController.pathAnimation
{
    public UnityEvent<CardPlayData> OnCardCompletionTrigger;
    public UnityEvent<CardPlayData> OnCardStartTrigger;
    public UnityEvent<GodDialogueTrigger, UnityEngine.Object> OnCardDrawDialogue;
    public UnityEvent<EventReference, GameObject> OnAnimStartSound;
    public UnityEvent<EventReference, GameObject> OnAnimEndSound;
    public CardPlayData _card;
    EventReference sound;
    GameObject soundTarget;
    GodDialogueTrigger _dialogueTrigger;

    public CardPathAnim(CardPlayData card, EventReference soundEvent, GameObject SoundEmitterTarget, GodDialogueTrigger dialogueTrigger)
    {
        OnCardCompletionTrigger = new UnityEvent<CardPlayData>();
        OnCardStartTrigger = new UnityEvent<CardPlayData>();
        OnCardDrawDialogue = new UnityEvent<GodDialogueTrigger, Object>();
        OnAnimStartSound = new UnityEvent<EventReference, GameObject>();
        OnAnimEndSound = new UnityEvent<EventReference, GameObject>();
        OnAnimCompletionTrigger = new UnityEvent();
        OnAnimStartTrigger = new UnityEvent();

        _card = card;
        sound = soundEvent;
        soundTarget = SoundEmitterTarget;
        _dialogueTrigger = dialogueTrigger;
    }

    public override void completionTrigger(string animationName)
    {
        OnCardDrawDialogue?.Invoke(_dialogueTrigger, _card.CardType);
        OnAnimEndSound?.Invoke(sound, soundTarget);
        OnCardCompletionTrigger?.Invoke(_card);
        OnAnimCompletionTrigger?.Invoke();
        _Complete = true;
        //Debug.Log("complete " + animationName);
    }

    public override void startTrigger()
    {
        OnAnimStartSound?.Invoke(sound, soundTarget);
        OnCardStartTrigger?.Invoke(_card);
        OnAnimStartTrigger?.Invoke();
        _Started = true;
    }
}
