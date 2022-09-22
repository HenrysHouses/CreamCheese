using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player_Hand))]
public class Hand_editor : Editor 
{
    private Player_Hand script;

    private void OnEnable()
    {
        script = target as Player_Hand;
    }



    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Add Card"))
        {
            GameObject obj = Instantiate(script.spawnCard, new Vector3(1.47000003f,-0.140000001f,-0.370000005f), Quaternion.identity);
            
            Player_Hand.CardInHand card = new Player_Hand.CardInHand(obj.GetComponentInChildren<Card_Selector>());
            script.AddCard(card);
        }    
    }
}
