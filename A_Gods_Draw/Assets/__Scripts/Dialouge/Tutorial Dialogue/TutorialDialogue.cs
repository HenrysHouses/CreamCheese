using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialDialogue : IDialogue
{
    public TutorialSentence[] TutorialPages;
    
    [HideInInspector]
    public override sentence[] pages 
    {
        get{ return TutorialPages as sentence[]; } 
        set{}
    }
    [field:SerializeField]
    public override string TransformName {get; set;} = "MainCamera";
    [field:SerializeField]
    public override EventReference SFX {get; set;}
}

[System.Serializable]
public class TutorialSentence : sentence
{
    [HideInInspector] public UnityEvent OnComplete = new UnityEvent();
    public TutorialStepTrigger StepTrigger;
    public bool IsComplete = false;

    /// <summary>Trigger this when the condition for continuing the page is met (try events)</summary>
    public void Complete()
    {
        IsComplete = true;
        OnComplete?.Invoke();

        if(StepTrigger == null)
            return;
        
        StepTrigger.OnTrigger.RemoveAllListeners();
    }

    public void WaitForTrigger()
    {
        if(StepTrigger == null)
            return;
        
        StepTrigger.OnTrigger.AddListener(Complete);
    }
}