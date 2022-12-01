/*
 * Written by:
 * Henrik
 * 
 */

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathController))]
public class PathController_Editor : Editor
{
   
    private PathController script;

    private void OnEnable()
    {
        script = target as PathController;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Recalculate"))
        {
            script.recalculatePath();
        }
    }
}
#endif