using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    #if UNITY_EDITOR
        [SerializeField] bool SkipBoot;        
    #endif

    void Awake()
    {
        #if !UNITY_EDITOR
            MultiSceneLoader.BootGame();
        #elif UNITY_EDITOR
            // decide if it should boot or not in the editor.
            if(!SkipBoot)
                MultiSceneLoader.BootGame();
        #endif
    }
}