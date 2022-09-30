using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[CreateAssetMenu(menuName = "Multi Scenes/SceneCollectionObject")]
public class SceneCollectionObject : ScriptableObject
{
    public string Title;

    [HideInInspector]
    public List<SceneAsset> Scenes = new List<SceneAsset>();

    #if UNITY_EDITOR
    public void saveCollection(SceneAsset[] scenes)
    {
        Scenes.Clear();
        for (int i = 0; i < scenes.Length; i++)
        {
            Scenes.Add(scenes[i]);
        }

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = this;
    }
    #endif
}