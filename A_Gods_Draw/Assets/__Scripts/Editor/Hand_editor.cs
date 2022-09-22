using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player_Hand))]
public class Hand_editor : Editor 
{
    private Player_Hand script;

    Object SO;

    private void OnEnable()
    {
        script = target as Player_Hand;
    }

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        EditorGUILayout.ObjectField(SO, typeof(Object), true);

        if(GUILayout.Button("Add Card"))
        {
            GameObject obj = Instantiate(script.CardinHandPrefab, new Vector3(1.47000003f,-0.140000001f,-0.370000005f), Quaternion.identity);
            
            Card_SO card = SO as Card_SO;

            script.AddCard(card);
        }    
    }
}
