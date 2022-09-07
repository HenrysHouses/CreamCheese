using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


[CustomEditor(typeof(PathAnimatorController))]
public class PathAnimator_Editor : Editor
{
   private PathAnimatorController script;

    private void OnEnable()
    {
        script = target as PathAnimatorController;
    }

    AnimationCurve curve = new AnimationCurve();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.CurveField("Speed Curve", curve);


        if(GUILayout.Button("Animate"))
        {
            PathAnimatorController.AnimatorMovement animation = new PathAnimatorController.AnimatorMovement(script._speed);
            animation.AnimationTarget = new GameObject("Test Target");
            animation.AnimationTarget.transform.SetParent(animation.AnimationTransform);
            animation.AnimationTarget.transform.position = new Vector3();   
            animation.index = script.CreateAnimation(animation);
            animation.CompletionTrigger.AddListener(script.debugTestCompletion);
        }
    }


}