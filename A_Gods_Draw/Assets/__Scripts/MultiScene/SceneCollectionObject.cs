using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Workflow/SceneCollectionObject")]
public class SceneCollectionObject : ScriptableObject
{
    public string Title;

    [HideInInspector] public List<string> Scenes = new List<string>();
    [HideInInspector] public List<int> ChoiceIndex = new List<int>();
    string[] _sceneOptions = new string[] {"None"};
    public string[] getSceneOptions() => _sceneOptions;

    void OnEnable()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        string[] scenes = new string[sceneCount];

        for( int i = 0; i < sceneCount; i++ )
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
        }
        _sceneOptions = scenes;
    }
}