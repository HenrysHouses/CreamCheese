//Charlie

using HH.MultiSceneTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightScenes : MonoBehaviour
{
    [SerializeField]
    CollectionLight[] collectionLights;

    // Start is called before the first frame update
    void Start()
    {
        SetLight(MultiSceneLoader.currentlyLoaded, collectionLoadMode.Difference);
        MultiSceneLoader.OnSceneCollectionLoaded.AddListener(SetLight);
    }

    private void SetLight(SceneCollection collection, collectionLoadMode mode)
    {
        for (int i = 0; i < collectionLights.Length; i++)
        {
            if (collectionLights[i].name == collection.Title)
            {
                for (int j = 0; j < collectionLights[i].lightSetting.Length; j++)
                {
                    collectionLights[i].lightSetting[j].light.GetComponent<ChangeColorLight>()
                        .SetLightSettings(collectionLights[i].lightSetting[j]);
                }
            }
        }
    }
}
