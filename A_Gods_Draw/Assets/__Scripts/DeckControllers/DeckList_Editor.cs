using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeckList_SO))]
public class DeckList_Editor : Editor
{
    DeckList_SO script;
    private void OnEnable() {
        script = target as DeckList_SO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate GUIDs"))
        {
            for (int i = 0; i < script.deckData.deckListData.Count; i++)
            {
                CardPlayData data = script.deckData.deckListData[i];
                data.Experience.createGUID();
                script.deckData.deckListData[i] = data;
            }
            EditorUtility.SetDirty(script);
        }
    }
}
