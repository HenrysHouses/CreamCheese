using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIntensityEffect : SceneIntensityEffect
{

    [SerializeField]
    private IntensityEffectSettings[] objectsToEnable;
    public struct IntensityEffectSettings
    {

        public GameObject Object;
        public float Threshold;
        public bool Enabled;

    }

    private void Start()
    {

        for(int i = 0; i < objectsToEnable.Length; i++)
            objectsToEnable[i].Object.SetActive(false);

    }

    public override void UpdateIntensity(float _intensity)
    {

        for(int i = 0; i < objectsToEnable.Length; i++)
        {

            if(_intensity >= objectsToEnable[i].Threshold)
                objectsToEnable[i].Object.SetActive(objectsToEnable[i].Enabled);
            else
                objectsToEnable[i].Object.SetActive(!objectsToEnable[i].Enabled);

        }

    }

}
