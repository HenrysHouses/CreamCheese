using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Multi Scenes/Editor Config"), ]
public class MultiSceneEditorConfig : ScriptableObject
{
    public static MultiSceneEditorConfig instance;

    [SerializeField] SceneCollectionObject currentLoadedCollection;
    public void setCurrCollection(SceneCollectionObject newCollection)
    {
        currentLoadedCollection = newCollection;
    }

    public SceneCollectionObject getCurrCollection()
    {
        if(currentLoadedCollection)
            return currentLoadedCollection;
        // Debug.LogWarning("No Current Loaded Collection Found");
        return null;
    }

    #if UNITY_EDITOR
    [InitializeOnLoadMethod]
    #endif
    public void setInstance()
    {
        if(!instance)    
            instance = this;
        else
            Debug.LogWarning("MultiSceneEditorConfig: Instance already set.");
    }

    private void OnEnable() {
        setInstance();
    }
}