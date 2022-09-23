using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationManager_SO))]
public class AnimationManager_Editor : Editor
{
    private AnimationManager_SO script;

    private void OnEnable()
    {
        script = target as AnimationManager_SO;
    }

   public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Trigger AnimationRequestChangeEvent"))
        {
            script.AnimationRequestChangeEvent?.Invoke();
        }
    }
}