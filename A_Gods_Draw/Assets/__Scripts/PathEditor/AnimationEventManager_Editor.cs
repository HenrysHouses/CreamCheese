using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AnimationEventManager))]
public class AnimationEventManager_Editor : Editor
{
    private AnimationEventManager script;

    private void OnEnable()
    {
        script = target as AnimationEventManager;
    }

   public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(AnimationEventManager.getInstance)
            GUILayout.TextField(AnimationEventManager.getInstance.name);

        // if(GUILayout.Button("Trigger AnimationRequestChangeEvent"))
        // {
        //     script.OnAnimationRequestChange?.Invoke();
        // }
    }
}

#endif