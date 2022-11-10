using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GodDialogue))]
public class GodDialogue_Editor : Editor
{
    private GodDialogue script;

    private SerializedProperty drawAmountProperty;

    private void OnEnable()
    {
        script = target as GodDialogue;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(!script.GenericTrigger)
        {
            script.DialogueName = GUILayout.TextField(
                // new GUIContent("Card", "What Special card triggers this dialogue"), 
                script.DialogueName);
        }
        else
            script.DialogueName = "";
    }
}