using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIntensityEffect : MonoBehaviour
{

    protected virtual void Start()
    {

        GameManager.instance.AddSceneIntensityEffect(this);

    }

    /// <summary>
    /// intensity goes from 0-1
    /// </summary>
    public virtual void UpdateIntensity(float _intensity, bool _updateFromDamage = false){}

    private void OnDestroy()
    {

        GameManager.instance.RemoveSceneIntensityEffect(this);

    }

}
