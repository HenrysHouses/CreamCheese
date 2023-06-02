using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
[CustomEditor(typeof(WavePlayer))]
public class WavePlayer_Editor : Editor
{
    WavePlayer script;
    private void OnEnable() {
        script = target as WavePlayer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Play All Waves"))
        {
                script.play();
        }
    }
}
#endif