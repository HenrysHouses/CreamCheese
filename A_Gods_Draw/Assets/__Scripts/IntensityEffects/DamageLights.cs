using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLights : SceneIntensityEffect
{

    [SerializeField]
    private GameObject[] lights;

    public override void UpdateIntensity(float _intensity, bool _updateFromDamage = false)
    {
        
        int _count = Mathf.RoundToInt(_intensity * ((float)lights.Length + 1f));
        for(int i = 0; i < lights.Length; i++)
            lights[i].SetActive(_count > i + 1 ? true : false);

    }

}
