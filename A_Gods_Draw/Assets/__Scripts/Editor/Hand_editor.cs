using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player_Hand2))]
public class Hand_editor : Editor 
{
    private Player_Hand2 script;

    private void OnEnable()
    {
        script = target as Player_Hand2;
    }



    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Add Card"))
        {
            GameObject obj = Instantiate(script.spawnCard, new Vector3(1.47000003f,-0.140000001f,-0.370000005f), Quaternion.identity);
            
            Player_Hand2.CardInHand card = new Player_Hand2.CardInHand(obj.GetComponentInChildren<Card_Selector>());
            script.AddCard(card);
        }    
    }
}
