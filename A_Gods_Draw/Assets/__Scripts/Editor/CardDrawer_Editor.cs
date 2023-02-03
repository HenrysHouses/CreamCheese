/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDrawer_debugger))]
public class CardDrawer_Editor : Editor
{
    private CardDrawer_debugger script;

    private SerializedProperty drawAmountProperty;

    private void OnEnable()
    {
        script = target as CardDrawer_debugger;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Add Card"))
        {
            if(EditorApplication.isPlaying)
            {
                script.addCardTest();
            }
            else
                Debug.Log("Editor needs to be in play to use this button");
        }

        if(GUILayout.Button("Reset library"))
        {
            if(EditorApplication.isPlaying)
            {
                script.createLibrary();
            }
            else
                Debug.Log("Editor needs to be in play to use this button");
        }

        if(GUILayout.Button("shuffle Library"))
        {
            if(EditorApplication.isPlaying)
            {
                script.Shuffle();
            }
            else
                Debug.Log("Editor needs to be in play to use this button");
        }

        if(GUILayout.Button("recycle discard"))
        {
            if(EditorApplication.isPlaying)
            {
                script.Recycle();
            }            
            else
                Debug.Log("Editor needs to be in play to use this button");
        }

        script.editor_drawAmount = EditorGUILayout.IntField("Draw X cards", script.editor_drawAmount);

        if(GUILayout.Button("Draw card"))
        {
            if(EditorApplication.isPlaying)
            {
                script.DrawACard(script.editor_drawAmount);
            }
            else
                Debug.Log("Editor needs to be in play to use this button");
        }

        if(GUILayout.Button("Discard card"))
        {
            if(EditorApplication.isPlaying)
            {
                script.DiscardACard();
            }
            else
                Debug.Log("Editor needs to be in play to use this button");
        }

        if(GUILayout.Button("Discard hand"))
        {
            if(EditorApplication.isPlaying)
            {
                Debug.Log("Pee Pee Poo Poo");
            }
            else
                Debug.Log("Editor needs to be in play to use this button");
        }
    }
}
