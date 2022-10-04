using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[CreateAssetMenu(menuName = "Multi Scenes/SceneCollectionObject")]
public class SceneCollectionObject : ScriptableObject
{
    public string Title;

    [HideInInspector] public List<string> SceneNames = new List<string>();

    #if UNITY_EDITOR
    [HideInInspector]
    public List<SceneAsset> Scenes = new List<SceneAsset>();
    public void saveCollection(SceneAsset[] scenes)
    {
        Scenes.Clear();
        SceneNames.Clear();
        for (int i = 0; i < scenes.Length; i++)
        {
            Scenes.Add(scenes[i]);
            SceneNames.Add(scenes[i].name);
        }

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = this;
    }

    private void OnValidate() 
    {
        SceneNames.Clear();
        for (int i = 0; i < Scenes.Count; i++)
        {
            SceneNames.Add(Scenes[i].name);
        }
    }
    #endif
}