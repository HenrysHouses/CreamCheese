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
    private bool actionListDropdown, useStrengthMod;
    private float sliderVal;
    private List<bool> actionDropdowns;

    private void OnEnable()
    {

        EnemyActions = serializedObject.FindProperty("EnemyActions");
        actionDropdowns = new List<bool>(EnemyActions.arraySize);
        actionListDropdown = false;

    }

    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Enemy Actions Setup");
        actionListDropdown = EditorGUILayout.Foldout(actionListDropdown, "Actions", true, EditorStyles.foldoutHeader);
        if(actionListDropdown)
        {

            EditorGUI.indentLevel++;

            for(int i = 0; i < EnemyActions.arraySize; i++)
            {

                EditorGUILayout.Space();

                if((actionDropdowns.Count - 1) < i)
                    actionDropdowns.Add(false);
                actionDropdowns[i] = EditorGUILayout.Foldout(actionDropdowns[i], "Action " + i, true, EditorStyles.foldoutHeader);

                if(!actionDropdowns[i])
                    continue;

                EditorGUI.indentLevel++;

                SerializedProperty _actionSelection = EnemyActions.GetArrayElementAtIndex(i);

                _actionSelection.Next(true);//Action type
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//Action conditions
                EditorGUILayout.PropertyField(_actionSelection);
                if(_actionSelection.arraySize > 1)
                {

                    _actionSelection.Next(false);//AllRequired
                    EditorGUILayout.PropertyField(_actionSelection);

                }
                else
                    _actionSelection.Next(false);//AllRequired

                EditorGUILayout.BeginHorizontal();
                _actionSelection.Next(false);//MinStrength
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//MaxStrength
                EditorGUILayout.PropertyField(_actionSelection);
                EditorGUILayout.EndHorizontal();

                useStrengthMod = EditorGUILayout.Toggle("Use Strength modifications", useStrengthMod);
                _actionSelection.Next(false);//UseStrengthMod
                _actionSelection.boolValue = useStrengthMod;
                if(useStrengthMod)
                {

                    EditorGUI.indentLevel++;
                    
                    _actionSelection.Next(false);//StrengthModConditions
                    EditorGUILayout.PropertyField(_actionSelection);

                    if(_actionSelection.arraySize > 1)
                    {

                        _actionSelection.Next(false);//AllRequired
                        EditorGUILayout.PropertyField(_actionSelection);

                    }
                    else
                        _actionSelection.Next(false);//AllRequired

                    _actionSelection.Next(false);//ModifiedStrength
                    EditorGUILayout.PropertyField(_actionSelection);

                    EditorGUI.indentLevel--;

                }
                else
                {

                    _actionSelection.Next(false);
                    _actionSelection.Next(false);
                    _actionSelection.Next(false);

                }

                _actionSelection.Next(false);//Weigth
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//Priority
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//TurnsToPerform
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//SFX
                EditorGUILayout.PropertyField(_actionSelection);

                EditorGUILayout.BeginHorizontal();

                if(GUILayout.Button("+", EditorStyles.miniButtonLeft))
                {

                    EnemyActions.InsertArrayElementAtIndex(EnemyActions.arraySize);
                    actionDropdowns.Add(true);

                }

                if(GUILayout.Button("-", EditorStyles.miniButtonRight))
                {
    
                    int _arraySize = EnemyActions.arraySize;
                    EnemyActions.DeleteArrayElementAtIndex(i);
                    actionDropdowns.RemoveAt(i);
                    if(EnemyActions.arraySize == _arraySize)
                        EnemyActions.DeleteArrayElementAtIndex(i);

                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;

            }

            EditorGUI.indentLevel--;

        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

    }

}