using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIntensityEffect : SceneIntensityEffect
{

    [SerializeField]
    private IntensityEffectSettings[] objectsToEnable;
    [System.Serializable]
    public struct IntensityEffectSettings
    {

        public GameObject VFXObject;
        public float Threshold;
        public bool Enabled;

    }

    private void Start()
    {

        for(int i = 0; i < objectsToEnable.Length; i++)
            objectsToEnable[i].VFXObject.SetActive(false);

    }

    public override void UpdateIntensity(float _intensity)
    {

        for(int i = 0; i < objectsToEnable.Length; i++)
        {

            if(_intensity >= objectsToEnable[i].Threshold)
                objectsToEnable[i].VFXObject.SetActive(objectsToEnable[i].Enabled);
            else
                objectsToEnable[i].VFXObject.SetActive(!objectsToEnable[i].Enabled);

        }

    }

}
