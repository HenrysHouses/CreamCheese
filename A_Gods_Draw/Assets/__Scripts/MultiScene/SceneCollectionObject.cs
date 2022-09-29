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
    public List<string> Scenes = new List<string>();

    [HideInInspector] 
    public List<int> ChoiceIndex = new List<int>();
    string[] _sceneOptions = new string[] {"None"};
    public string[] getSceneOptions() => _sceneOptions;
    public void setSceneOptions(string[] options) => _sceneOptions = options;

    void OnEnable()
    {
    }

    void OnValidate()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;     
        Debug.Log(sceneCount);
        string[] scenes = new string[sceneCount];

        for( int i = 0; i < sceneCount; i++ )
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension( SceneUtility.GetScenePathByBuildIndex( i ) );
        }
        _sceneOptions = scenes;

    }

    #if UNITY_EDITOR

    public void saveCollection(string[] scenes)
    {
        Scenes.Clear();
        ChoiceIndex.Clear();
        for (int i = 0; i < scenes.Length; i++)
        {
            Scenes.Add(scenes[i]);
            int sceneCount = SceneManager.sceneCountInBuildSettings;     
            for( int j = 0; j < sceneCount; j++ )
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( j ) );
                // Debug.Log(sceneName + " ?= " + scenes[i]);
                if(sceneName.Equals(scenes[i]))
                    ChoiceIndex.Add(j);        
            }
        }

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = this;
    }
    #endif
}