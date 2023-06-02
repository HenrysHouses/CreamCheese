
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShockWavePlayer))]
public class ShockWavePlayer_Editor : Editor
{
    ShockWavePlayer script;
    private void OnEnable() {
        script = target as ShockWavePlayer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Play"))
        {
            script.play();
        }
    }
}
#endif