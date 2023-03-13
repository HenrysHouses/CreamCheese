using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
public class CameraTutorial : TutorialController
{

    public override void CheckTutorialConditions()
    {
        if(isTutorialStep(0))
            startTutorialRoutine(PressAnyButtonToContinue(PressAnyButtonToContinueTrigger, 0));

        if(isTutorialStep(1))
            startTutorialRoutine(PressAllCameraButtons());
    }

    public override void OnAllStepsComplete()
    {
        MultiSceneLoader.loadCollection("CombatTutorial", collectionLoadMode.Difference);
        Debug.Log("completed camera tutorial");
    }

    public TutorialStepTrigger PressAnyButtonToContinueTrigger;

    public TutorialStepTrigger PressAllCameraButtonsTrigger;
    IEnumerator PressAllCameraButtons()
    {
        bool W = false, A = false ,S = false ,D = false;
        
        while(!W || !A || !S || !D)
        {
            if(Input.GetKey(KeyCode.W))
                W = true;

            if(Input.GetKey(KeyCode.A))
                A = true;

            if(Input.GetKey(KeyCode.S))
                S = true;

            if(Input.GetKey(KeyCode.D))
                D = true;

            yield return new WaitForEndOfFrame();
        }
        CameraMovement.instance.SetCameraView(CameraView.Reset);
        completeTutorialRoutine(PressAllCameraButtonsTrigger, 1);
    }
}
