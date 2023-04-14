using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Monster))]
[CanEditMultipleObjects]
public class MonsterEditor : Editor
{

    //SerializedProperty EnemyActions;

    private void OnEnable()
    {

        //EnemyActions = serializedObject.FindProperty("EnemyActions");

    }

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();

        /*serializedObject.Update();
        EditorGUILayout.PropertyField(EnemyActions);
        serializedObject.ApplyModifiedProperties();*/
        EditorGUILayout.LabelField("peppo");

    }

}