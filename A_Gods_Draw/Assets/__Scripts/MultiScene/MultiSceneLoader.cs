using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum collectionLoadMode
{
    difference,
    Replace,
    Keep
}

public class MultiSceneLoader : MonoBehaviour
{
    public static MultiSceneLoader instance;
    SceneCollectionObject[] Collection;
    [SerializeField] SceneCollectionObject BootCollection;
    [SerializeField] SceneCollectionObject currentlyLoaded;


    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Collection = Resources.LoadAll<SceneCollectionObject>("SceneCollections");

        #if !UNITY_EDITOR
            loadCollection(BootCollection.Title, collectionLoadMode.difference);
        #endif


    }

    public void loadCollection(string CollectionTitle, collectionLoadMode mode)
    {
        SceneCollectionObject TargetCollection = null;
        foreach (SceneCollectionObject target in Collection)
        {
            if(target.Title.Equals(CollectionTitle))
                TargetCollection = target;
        }

        if(TargetCollection == null)
            return;

        switch(mode)
        {
            case collectionLoadMode.difference:
                loadDifferenceFromCurrent(TargetCollection);
                break;

            case collectionLoadMode.Replace:
                
                break;

            case collectionLoadMode.Keep:
                
                break;
        }
    }

    void loadDifferenceFromCurrent(SceneCollectionObject Collection)
    {
        Debug.Log("load difference: " + currentlyLoaded.Title + ", " + Collection.Title);


        // Unload Differences
        foreach (string LoadedScene in currentlyLoaded.SceneNames)
        {
            bool difference = true;
            foreach (string targetScene in Collection.SceneNames)
            {
                if(LoadedScene.Equals(targetScene))
                {
                    difference = false;
                }
            }
            if(difference)
                unload(LoadedScene);
        }
        // load Differences
        foreach (string targetScene in Collection.SceneNames)
        {
            bool difference = true;
            foreach (string LoadedScene in currentlyLoaded.SceneNames)
            {
                if(targetScene.Equals(LoadedScene))
                {
                    difference = false;
                    // Debug.Log("pls load: " + targetScene);
                }
            }
            if(difference)
                load(targetScene, LoadSceneMode.Additive);
        }

        currentlyLoaded = Collection;
    }

    void unload(string SceneName)
    {
        SceneManager.UnloadSceneAsync(SceneName);
    }

    void load(string SceneName, LoadSceneMode mode)
    {
        Debug.Log(SceneName);
        SceneManager.LoadScene(SceneName, mode);
    }
}
