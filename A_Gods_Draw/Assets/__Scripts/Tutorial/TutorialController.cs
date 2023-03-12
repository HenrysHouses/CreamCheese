using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialController : MonoBehaviour
{
    public DialogueBox CurrentDialogue;
    public TutorialDialogue TutorialSteps;
    Coroutine currentRoutine;
    TutorialSentence currentStep;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        spawnTutorial();
    }
    
    protected virtual void Update()
    {
        if(currentStep.IsComplete)
        {
            nextTutPage();
        }

        CheckTutorialConditions();
    }

    void spawnTutorial()
    {
        CurrentDialogue = DialogueController.instance.SpawnDialogue(TutorialSteps, true);
        currentStep = CurrentDialogue.getCurrentPage() as TutorialSentence;
        currentStep.WaitForTrigger();
    } 

    void nextTutPage()
    {
        currentStep = CurrentDialogue.getCurrentPage() as TutorialSentence;

        if(currentStep == null)
        {
            this.enabled = false;

            OnAllStepsComplete();
            return;
        }

        currentStep.WaitForTrigger();
    }

    public bool isTutorialStep(int step)
    {
        if(CurrentDialogue.getCurrentPageIndex() == step && currentRoutine == null)
        {
            TutorialSentence sentence = CurrentDialogue.getCurrentPage() as TutorialSentence;

            if(sentence.IsComplete)
                return false;

            return true;
        }
        return false;
    }

    public void startTutorialRoutine(IEnumerator tutorial)
    {
        currentRoutine = StartCoroutine(tutorial);
    }

    public void completeTutorialRoutine(TutorialStepTrigger trigger, int pageSkip)
    {
        CurrentDialogue.skipPage(pageSkip);
        trigger.Trigger();
        StopCoroutine(currentRoutine);
        currentRoutine = null;
    }

    public abstract void CheckTutorialConditions();
    public virtual void OnAllStepsComplete(){}


    protected IEnumerator PressAnyButtonToContinue(TutorialStepTrigger trigger, int pageSkip)
    {
        bool isAllowedToContinue = false;
        bool previousState = CurrentDialogue.fullPageIsDisplaying;

        while(isAllowedToContinue == false)
        {
            yield return new WaitForEndOfFrame();

            if(CurrentDialogue.fullPageIsDisplaying == previousState && CurrentDialogue.fullPageIsDisplaying)
            {
                isAllowedToContinue = true;
            }
            previousState = CurrentDialogue.fullPageIsDisplaying;
        }

        yield return new WaitUntil(() => Input.anyKeyDown); 

        completeTutorialRoutine(trigger, pageSkip);
    }

    void OnDestroy()
    {
        if(CurrentDialogue)
            Destroy(CurrentDialogue.gameObject);
    }
}