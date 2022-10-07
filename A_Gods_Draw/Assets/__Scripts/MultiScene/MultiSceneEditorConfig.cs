using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Multi Scenes/Editor Config")]
public class MultiSceneEditorConfig : ScriptableObject
{
    public static MultiSceneEditorConfig instance;
    public SceneCollectionObject currentLoadedCollection;

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