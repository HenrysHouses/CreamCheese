/*
 * Written by:
 * Henrik
*/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PathAnimatorController))]
public class PathAnimator_Editor : Editor
{
   private PathAnimatorController script;

    AnimationCurve curve;

    private void OnEnable()
    {
        script = target as PathAnimatorController;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.FloatField(new GUIContent("Animation Length:", "Time in seconds"), script.getAnimLength(), EditorStyles.boldLabel);

        base.OnInspectorGUI();

        if(GUILayout.Button("Animate"))
        {
            if(EditorApplication.isPlaying)
            {
                GameObject card = Instantiate(script.testAnimationObj);
                AnimationEventManager.getInstance.requestAnimation(script.AnimationName, card);
            }
            else
                Debug.LogWarning("Play the editor to test the animation");
        }
    }
}
#endif