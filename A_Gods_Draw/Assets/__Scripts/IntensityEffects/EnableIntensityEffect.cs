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
        public bool Enable;
        [Tooltip("if false, the effect can be triggered at the start of the scene without taking damage first")]
        public bool ActivateOnDamage, ShouldLoop;

    }
    private float prevIntensity;

    protected override void Start()
    {

        base.Start();

        for(int i = 0; i < objectsToEnable.Length; i++)
            objectsToEnable[i].VFXObject.SetActive(false);

    }

    public override void UpdateIntensity(float _intensity, bool _updateFromDamage = false)
    {

        for(int i = 0; i < objectsToEnable.Length; i++)
        {

            if((objectsToEnable[i].ActivateOnDamage && !_updateFromDamage) || (!objectsToEnable[i].ShouldLoop && !_updateFromDamage))
                continue;

            if(_intensity >= objectsToEnable[i].Threshold /*&& objectsToEnable[i].Threshold > prevIntensity*/)
                objectsToEnable[i].VFXObject.SetActive(objectsToEnable[i].Enable);
            else
                objectsToEnable[i].VFXObject.SetActive(!objectsToEnable[i].Enable);

        }

        prevIntensity = _intensity;

    }

}
