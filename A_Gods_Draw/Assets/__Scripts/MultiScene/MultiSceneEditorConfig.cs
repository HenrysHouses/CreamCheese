using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Multi Scenes/Editor Config")]
public class MultiSceneEditorConfig : ScriptableObject
{
    public static MultiSceneEditorConfig instance;
    SceneCollectionObject currentLoadedCollection;
    public void setCurrCollection(SceneCollectionObject newCollection)
    {
        currentLoadedCollection = newCollection;
    }

    public SceneCollectionObject getCurrCollection()
    {
        if(currentLoadedCollection)
            return currentLoadedCollection;
        Debug.LogError("No Current Loaded Collection Found");
        return null;
    }

    public void setInstance()
    {
        if(!instance)    
            instance = this;
        else
            Debug.LogWarning("There are multiple MultiSceneEditorConfig.asset's. Please remove duplicates.");
    }

    private void OnEnable() {
        setInstance();
    }
}