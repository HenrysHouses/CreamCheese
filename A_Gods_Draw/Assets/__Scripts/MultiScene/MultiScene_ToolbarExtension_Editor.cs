#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MultiScene_ToolbarExtension_Editor
{

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if(GUILayout.Button(new GUIContent("Useless Button", "Button!!")))
        {
            
        }
    }
}
#endif