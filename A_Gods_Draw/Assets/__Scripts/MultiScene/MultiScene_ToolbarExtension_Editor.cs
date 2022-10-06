#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

[InitializeOnLoad]
public class MultiScene_ToolbarExtension_Editor
{
    static MultiScene_ToolbarExtension_Editor()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if(GUILayout.Button(new GUIContent("Useless Button", "Button!!")))
        {
            
        }
    }
}
#endif