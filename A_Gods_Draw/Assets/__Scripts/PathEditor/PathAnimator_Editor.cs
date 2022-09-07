/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


[CustomEditor(typeof(PathAnimatorController))]
public class PathAnimator_Editor : Editor
{
   private PathAnimatorController script;

    AnimationCurve curve;

    private void OnEnable()
    {
        script = target as PathAnimatorController;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Animate"))
        {
            if(EditorApplication.isPlaying)
            {
                PathAnimatorController.pathAnimation animation = new PathAnimatorController.pathAnimation();
                animation.AnimationTarget = Instantiate(script.testAnimationobj);
                // animation.AnimationTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // animation.AnimationTarget.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                animation.AnimationTarget.transform.SetParent(animation.AnimationTransform);
                animation.AnimationTarget.transform.position = new Vector3();   
                animation.speedCurve = script._speedCurve;
                animation.speedMultiplier = script.Multiplier;
                animation.index = script.CreateAnimation(animation);
                animation.CompletionTrigger.AddListener(script.debugTestCompletion);
            }
            else
                Debug.LogWarning("Play the editor to test the animation");
        }
    }
}