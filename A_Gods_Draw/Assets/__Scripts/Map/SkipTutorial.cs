using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    public void ExitTutorial()
    {
        if(GameManager.instance.shouldGenerateNewMap)
        {
            StartCoroutine(LoadingScreen.Instance.EnterLoadingScreen("StarterDeck", HH.MultiSceneTools.collectionLoadMode.Difference));
        }
        else
            StartCoroutine(LoadingScreen.Instance.EnterLoadingScreen("MainMenu", HH.MultiSceneTools.collectionLoadMode.Difference));
    }
}
