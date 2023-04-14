using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnemyAIEnums;

[CustomEditor(typeof(Monster))]
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

        base.OnInspectorGUI();
        /*serializedObject.Update();
        
        EditorGUILayout.LabelField("Enemy Actions Setup");
        EditorGUILayout.EnumPopup(actionSelected);
        actionsDropdown = EditorGUILayout.Foldout(actionsDropdown, "Actions", true);
        if(actionsDropdown)
        {

            for(int i = 0; i < EnemyActions.arraySize; i++)
            {

                SerializedProperty _actionSelection = EnemyActions.FindPropertyRelative("");

                foreach (string _s in EnemyActions.)
                {

                    EditorGUILayout.LabelField(_s);
                    
                }

            }

        }*/

    }

}
