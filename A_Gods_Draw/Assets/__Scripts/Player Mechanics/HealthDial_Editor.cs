#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(HealthCounterController))]
public class HealthDial_Editor : Editor {
    public HealthCounterController script;
    
    private void OnEnable() {
        script = target as HealthCounterController;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        // if(EditorGUILayout.LinkButton("Update dial"))
        //     script.checkPlayerStatus();
    }
}
#endif