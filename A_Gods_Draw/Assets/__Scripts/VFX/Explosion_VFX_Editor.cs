using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Explosion_VFX))]
public class Explosion_VFX_Editor : Editor {
    Explosion_VFX script;
    private void OnEnable() {
        script = target as Explosion_VFX;
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