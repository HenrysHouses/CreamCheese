#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManager_Editor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Clear PlayerPrefs"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif