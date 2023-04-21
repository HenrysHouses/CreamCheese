using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIntensityEffect : MonoBehaviour
{

    private void Start()
    {

        GameManager.instance.AddSceneIntensityEffect(this);

    }

    public virtual void UpdateIntensity(float _intensity){}

    private void OnDestroy()
    {

        GameManager.instance.RemoveSceneIntensityEffect(this);

    }

}
