using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    void Awake()
    {
        #if !UNITY_EDITOR
        MultiSceneLoader.BootGame();
        #endif
    }
}
