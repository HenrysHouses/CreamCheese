using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnemyAIEnums;

/*[CustomEditor(typeof(Monster))]
public class Monster_Editor : Editor
{

    SerializedProperty EnemyActions;
    private EnemyIntent actionSelected;
    private bool actionsDropdown;
    private float sliderVal;

    private void OnEnable()
    {

        EnemyActions = serializedObject.FindProperty("EnemyActions");
        actionsDropdown = false;

    }

    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Enemy Actions Setup");
        actionsDropdown = EditorGUILayout.Foldout(actionsDropdown, "Actions", true);
        if(actionsDropdown)
        {

            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField(EnemyActions.displayName);

            EnemyActions.Next(true);
            do
            {

                EditorGUILayout.LabelField(EnemyActions.displayName);

            } while(EnemyActions.Next(false));

            EnemyActions.Reset();

            EditorGUI.indentLevel--;

        }

    }

}
*/