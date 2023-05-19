using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
using HH.MultiSceneTools.Examples;

public class PressButtonToContinue : MonoBehaviour
{
    [SerializeField] string MainMenuCollectionTitle = "MainMenu";

    [SerializeField] GameObject NewGameButton, ContinueButton, PressToContinueText;

    bool shouldLoad = false;

    void Start()
    {
        Map.Map currentSave;

        if(Map.Map_Manager.loadMap(out currentSave))
        {
            NewGameButton.SetActive(true);
            ContinueButton.SetActive(true);
            PressToContinueText.SetActive(false);
            gameObject.SetActive(false);
        }
        else
            GameManager.instance.shouldGenerateNewMap = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.activeSelf)
            return;

        if(Input.anyKeyDown && !LoadingScreen.IsAnimating)
        {
            StartCoroutine(LoadingScreen.Instance.EnterLoadingScreen());
            shouldLoad = true;
        }

        if(!LoadingScreen.IsAnimating && shouldLoad)
            MultiSceneLoader.loadCollection(MainMenuCollectionTitle, collectionLoadMode.Replace);
    }
}
