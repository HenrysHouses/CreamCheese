using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class TutorialStepTrigger : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnTrigger = new UnityEvent();

    public void Trigger()
    {
        OnTrigger?.Invoke();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(TutorialStepTrigger))]
public class TutorialStepTrigger_Editor : Editor {

    TutorialStepTrigger script;
    private void OnEnable() {
        script = target as TutorialStepTrigger;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Invoke Trigger"))
        {
            script.OnTrigger?.Invoke();
        }
    }
}
#endif