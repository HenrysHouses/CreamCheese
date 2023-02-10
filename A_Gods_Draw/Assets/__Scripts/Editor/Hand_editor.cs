/* 
 * Written by 
 * Henrik
*/
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

        SO = EditorGUILayout.ObjectField(new GUIContent("Card ScriptObj", "This card will be spawned in the hand"), SO, typeof(Card_SO), true);

        if(GUILayout.Button("Add Card"))
        {
            Card_SO card = SO as Card_SO;

            CardPlayData data = new CardPlayData();
            data.CardType = card;

            script.AddCard(data);
        }    
    }
}
