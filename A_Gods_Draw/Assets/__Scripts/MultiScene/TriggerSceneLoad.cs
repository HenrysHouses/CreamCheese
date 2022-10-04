using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSceneLoad : MonoBehaviour
{
    public void LoadScene(string collectionTitle)
    {
        MultiSceneLoader.instance.loadCollection(collectionTitle, MultiSceneLoader.collectionLoadMode.difference);
    }
}
