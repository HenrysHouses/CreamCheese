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
    private bool actionListDropdown, useStrengthMod, useWeigthMod;
    private float sliderVal;
    private List<bool> actionDropdowns, actionCondDd, strengthCondDd, weightCondDd;

    private void OnEnable()
    {

        EnemyActions = serializedObject.FindProperty("EnemyActions");
        actionDropdowns = new List<bool>(EnemyActions.arraySize);
        actionCondDd = new List<bool>(EnemyActions.arraySize);
        strengthCondDd = new List<bool>(EnemyActions.arraySize);
        weightCondDd = new List<bool>(EnemyActions.arraySize);
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

            if(EnemyActions.arraySize == 0)
            {

                if(GUILayout.Button("+", EditorStyles.miniButtonMid))
                {

                    EnemyActions.InsertArrayElementAtIndex(EnemyActions.arraySize);
                    actionDropdowns.Add(true);

                }

            }

            for(int i = 0; i < EnemyActions.arraySize; i++)
            {

                EditorGUILayout.Space(20);

                if((actionDropdowns.Count - 1) < i)
                    actionDropdowns.Add(false);
                
                GUILayout.BeginVertical("Action " + i, "window");
                actionDropdowns[i] = EditorGUILayout.Foldout(actionDropdowns[i], "Action " + i, true, EditorStyles.foldoutHeader);

                SerializedProperty _actionSelection = EnemyActions.GetArrayElementAtIndex(i);

                _actionSelection.Next(true);//Action type
                EditorGUILayout.PropertyField(_actionSelection);

                if(!actionDropdowns[i])
                {

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
                    GUILayout.EndVertical();
                    continue;

                }

                EditorGUI.indentLevel++;

                _actionSelection.Next(false);//Action conditions

                SerializedProperty _actionConditions = _actionSelection;
                if(_actionConditions.arraySize == 0)
                        _actionConditions.InsertArrayElementAtIndex(_actionConditions.arraySize);

                while(actionCondDd.Count-1 < i)
                    actionCondDd.Add(false);

                actionCondDd[i] = EditorGUILayout.Foldout(actionCondDd[i], "Action Conditions", true, EditorStyles.foldoutHeader);

                if(actionCondDd[i])
                {

                    EditorGUI.indentLevel++;
                    GUILayout.BeginVertical("Conditions", "window");

                    for(int j = 0; j < _actionConditions.arraySize; j++)
                    {

                        EditorGUILayout.LabelField("Element: " + j);

                        SerializedProperty _actionCondition = _actionConditions.GetArrayElementAtIndex(j);
                        _actionCondition.Next(true);
                        EditorGUILayout.PropertyField(_actionCondition);
                        _actionCondition.Next(false);
                        EditorGUILayout.PropertyField(_actionCondition);

                        EditorGUILayout.BeginHorizontal();

                        if(GUILayout.Button("+", EditorStyles.miniButtonLeft))
                        {

                            _actionConditions.InsertArrayElementAtIndex(_actionConditions.arraySize);

                        }

                        if(GUILayout.Button("-", EditorStyles.miniButtonRight))
                        {
            
                            int _arraySize = _actionConditions.arraySize;
                            _actionConditions.DeleteArrayElementAtIndex(j);
                            if(_actionConditions.arraySize == _arraySize)
                                _actionConditions.DeleteArrayElementAtIndex(j);

                        }

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                    }

                    GUILayout.EndVertical();
                    EditorGUI.indentLevel--;

                }

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

                _actionSelection.Next(false);//UseStrengthMod
                _actionSelection.boolValue = EditorGUILayout.Toggle("Use Strength modifications", _actionSelection.boolValue);
                if(_actionSelection.boolValue)
                {

                    EditorGUI.indentLevel++;
                    GUILayout.BeginVertical("Strength Mod Settings", "window");
                    
                    _actionSelection.Next(false);//StrengthModConditions

                    SerializedProperty _strengthConditions = _actionSelection;
                    if(_strengthConditions.arraySize == 0)
                        _strengthConditions.InsertArrayElementAtIndex(_strengthConditions.arraySize);

                    while(strengthCondDd.Count-1 < i)
                        strengthCondDd.Add(false);

                    strengthCondDd[i] = EditorGUILayout.Foldout(strengthCondDd[i], "Action Conditions", true, EditorStyles.foldoutHeader);

                    if(strengthCondDd[i])
                    {

                        EditorGUI.indentLevel++;
                        GUILayout.BeginVertical("Conditions", "window");

                        for(int j = 0; j < _strengthConditions.arraySize; j++)
                        {

                            EditorGUILayout.LabelField("Element: " + j);

                            SerializedProperty _strengthCondition = _strengthConditions.GetArrayElementAtIndex(j);
                            _strengthCondition.Next(true);
                            EditorGUILayout.PropertyField(_strengthCondition);
                            _strengthCondition.Next(false);
                            EditorGUILayout.PropertyField(_strengthCondition);

                            EditorGUILayout.BeginHorizontal();

                            if(GUILayout.Button("+", EditorStyles.miniButtonLeft))
                            {

                                _strengthConditions.InsertArrayElementAtIndex(_strengthConditions.arraySize);

                            }

                            if(GUILayout.Button("-", EditorStyles.miniButtonRight))
                            {
                
                                int _arraySize = _strengthConditions.arraySize;
                                _strengthConditions.DeleteArrayElementAtIndex(j);
                                if(_strengthConditions.arraySize == _arraySize)
                                    _strengthConditions.DeleteArrayElementAtIndex(j);

                            }

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                        }

                        GUILayout.EndVertical();
                        EditorGUI.indentLevel--;

                    }

                    if(_actionSelection.arraySize > 1)
                    {

                        _actionSelection.Next(false);//AllRequired
                        EditorGUILayout.PropertyField(_actionSelection);

                    }
                    else
                        _actionSelection.Next(false);//AllRequired

                    _actionSelection.Next(false);//ModifiedStrength
                    EditorGUILayout.PropertyField(_actionSelection);

                    GUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();

                }
                else
                {

                    _actionSelection.Next(false);
                    _actionSelection.Next(false);
                    _actionSelection.Next(false);

                }

                _actionSelection.Next(false);//UseWeigthMod
                _actionSelection.boolValue = EditorGUILayout.Toggle("Use Weigth modifications", _actionSelection.boolValue);
                if(_actionSelection.boolValue)
                {

                    EditorGUI.indentLevel++;
                    GUILayout.BeginVertical("Weigth Mod Settings", "window");
                    
                    _actionSelection.Next(false);//weigthModConditions
                    
                    SerializedProperty _weightConditions = _actionSelection;
                    if(_weightConditions.arraySize == 0)
                        _weightConditions.InsertArrayElementAtIndex(_weightConditions.arraySize);

                    while(weightCondDd.Count-1 < i)
                        weightCondDd.Add(false);

                    weightCondDd[i] = EditorGUILayout.Foldout(weightCondDd[i], "Action Conditions", true, EditorStyles.foldoutHeader);

                    if(weightCondDd[i])
                    {

                        EditorGUI.indentLevel++;
                        GUILayout.BeginVertical("Conditions", "window");

                        for(int j = 0; j < _weightConditions.arraySize; j++)
                        {

                            EditorGUILayout.LabelField("Element: " + j);

                            SerializedProperty _weightCondition = _weightConditions.GetArrayElementAtIndex(j);
                            _weightCondition.Next(true);
                            EditorGUILayout.PropertyField(_weightCondition);
                            _weightCondition.Next(false);
                            EditorGUILayout.PropertyField(_weightCondition);

                            EditorGUILayout.BeginHorizontal();

                            if(GUILayout.Button("+", EditorStyles.miniButtonLeft))
                            {

                                _weightConditions.InsertArrayElementAtIndex(_weightConditions.arraySize);

                            }

                            if(GUILayout.Button("-", EditorStyles.miniButtonRight))
                            {
                
                                int _arraySize = _weightConditions.arraySize;
                                _weightConditions.DeleteArrayElementAtIndex(j);
                                if(_weightConditions.arraySize == _arraySize)
                                    _weightConditions.DeleteArrayElementAtIndex(j);

                            }

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                        }

                        GUILayout.EndVertical();
                        EditorGUI.indentLevel--;

                    }

                    if(_actionSelection.arraySize > 1)
                    {

                        _actionSelection.Next(false);//AllRequired
                        EditorGUILayout.PropertyField(_actionSelection);

                    }
                    else
                        _actionSelection.Next(false);//AllRequired

                    
                    _actionSelection.Next(false);//clearOnFalse
                    EditorGUILayout.PropertyField(_actionSelection);
                    _actionSelection.Next(false);//ModifiedWeigth
                    EditorGUILayout.PropertyField(_actionSelection);

                    GUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();

                }
                else
                {

                    _actionSelection.Next(false);
                    _actionSelection.Next(false);
                    _actionSelection.Next(false);
                    _actionSelection.Next(false);

                }

                _actionSelection.Next(false);//AddedWeigth;

                _actionSelection.Next(false);//Weigth
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//Priority
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//TurnsToPerform
                EditorGUILayout.PropertyField(_actionSelection);
                _actionSelection.Next(false);//VFX
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

                GUILayout.EndVertical();
                EditorGUI.indentLevel--;

            }

            EditorGUI.indentLevel--;

        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

    }

}