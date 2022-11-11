#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GodDialogue))]
public class GodDialogue_Editor : Editor
{
    private GodDialogue script;
    
    private SerializedProperty serializedProperty;

    private void OnEnable()
    {
        script = target as GodDialogue;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(!script.GenericTrigger)
        {
            script.cardTrigger = (Card_SO)EditorGUILayout.ObjectField(
                new GUIContent("Card Trigger", "The card that triggers this dialogue"), 
                script.cardTrigger, typeof(Card_SO), false);
        }
        else
        {
            script.chanceToPlay = EditorGUILayout.Slider(
                new GUIContent("Chance To Play", "Chance in percent (%)"), 
                script.chanceToPlay, 0, 1);
        }
    }
}
#endif