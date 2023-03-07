using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenTrigger : MonoBehaviour
{
    public void triggerLoadingScreen(string loadscene)
    {
        StartCoroutine(LoadingScreen.Instance.EnterLoadingScreen(loadscene, HH.MultiSceneTools.collectionLoadMode.Difference));
    }
}
