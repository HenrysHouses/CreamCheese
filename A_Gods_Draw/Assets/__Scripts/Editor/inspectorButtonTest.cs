/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDrawer_debugger))]
public class inspectorButtonTest : Editor
{
    private CardDrawer_debugger script;

    private void OnEnable()
    {
        script = target as CardDrawer_debugger;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Add Card"))
        {
            script.addCardTest();
        }

        if(GUILayout.Button("Reset library"))
        {
            script.createLibrary();
        }

        if(GUILayout.Button("shuffle Library"))
        {
            script.Shuffle();
        }

        if(GUILayout.Button("recycle discard"))
        {
            script.Recycle();
        }

        if(GUILayout.Button("Draw card"))
        {
            script.DrawACard(1);
        }

        if(GUILayout.Button("Discard card"))
        {
            script.DiscardACard();
        }

        if(GUILayout.Button("Discard hand"))
        {
            script.DiscardHand();
        }
    }
}
