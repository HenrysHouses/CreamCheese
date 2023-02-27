using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
using HH.MultiSceneTools.Examples;

public class PressButtonToContinue : MonoBehaviour
{
    [SerializeField] string MainMenuCollectionTitle = "MainMenu";

    [SerializeField] SceneTransition transition;

    bool shouldLoad = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !LoadingScreen.IsAnimating)
        {
            StartCoroutine(LoadingScreen.Instance.EnterLoadingScreen());
            shouldLoad = true;
        }

        if(!LoadingScreen.IsAnimating && shouldLoad)
            MultiSceneLoader.loadCollection(MainMenuCollectionTitle, collectionLoadMode.Replace);
    }
}
