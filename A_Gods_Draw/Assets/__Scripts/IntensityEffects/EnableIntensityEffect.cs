using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIntensityEffect : SceneIntensityEffect
{

    [SerializeField]
    private float intensityThreshold;
    [SerializeField]
    private GameObject[] objectsToEnable;

    private void Start()
    {

        for(int i = 0; i < objectsToEnable.Length; i++)
            objectsToEnable[i].SetActive(false);

    }

    public override void UpdateIntensity(float _intensity)
    {
        
        if(intensityThreshold > _intensity)
            return;

        for(int i = 0; i < objectsToEnable.Length; i++)
            objectsToEnable[i].SetActive(true);

    }

}
