#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeckManager_SO))]
public class DeckManager_Editor : Editor 
{
    DeckManager_SO script;
    private void OnEnable()
    {
        script = target as DeckManager_SO;
    }
    
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Save Deck"))
        {
            //script.SavingDeck();
            GameSaver.SaveData(script.getDeck);
        }
    }
}
#endif