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

[CustomEditor(typeof(CardSearchTester))]
public class CardSearchTester_Editor : Editor
{
    CardSearchTester script;

    private void OnEnable() {
        script = target as CardSearchTester;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Search"))
        {
            script.searchCards();
        } 
    }
}
#endif