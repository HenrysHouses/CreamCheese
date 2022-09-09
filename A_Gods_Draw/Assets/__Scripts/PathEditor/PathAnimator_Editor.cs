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
                GameObject card = Instantiate(script.testAnimationObj);
                script.getAnimManagerSO().requestAnimation(script.PathName, card);
            }
            else
                Debug.LogWarning("Play the editor to test the animation");
        }
    }
}