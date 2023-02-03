// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;

// [CustomEditor(typeof(CardStats))]
// public class CardStats_Editor : Editor
// {
//     private object script;
//     private CardStats stats;

//     private void OnEnable()
//     {
//         script = target as object;
//     }

//     public override void OnInspectorGUI()
//     {
//         stats = script as CardStats;

//         // stats.SelectionTypeIndex = EditorGUILayout.Popup(stats.SelectionTypeIndex, BoardElementClassNames.instance.Names);


//         base.OnInspectorGUI();
//         // if(GUILayout.Button("Trigger AnimationRequestChangeEvent"))
//         // {
//         //     script.OnAnimationRequestChange?.Invoke();
//         // }
//     }
// }

// #endif