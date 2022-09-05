using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PathAnimator))]
public class Editor_PathAnimator : Editor
{
   private PathAnimator script;

    private void OnEnable()
    {
        script = target as PathAnimator;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Animate"))
        {
            script.CreateAnimation(new GameObject("TestAnimation"));
        }
    }
}