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
            GameSaver.SavePlayerData(new PlayerDataContainer(0, 30, new RuneData[0]));
        }
    }
}
#endif