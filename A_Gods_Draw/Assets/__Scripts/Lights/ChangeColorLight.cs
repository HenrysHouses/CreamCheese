//Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorLight : MonoBehaviour
{
    public EnvironmentLightSettings lightSettings;

    // Start is called before the first frame update
    void Start()
    {
        lightSettings.light = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightSettings.light.color = Color.Lerp(lightSettings.color, lightSettings.light.color, Time.deltaTime * lightSettings.speed);
        lightSettings.light.intensity = Mathf.Lerp(lightSettings.intensity, lightSettings.light.intensity, Time.deltaTime * lightSettings.speed); 
    }

    public void SetLightSettings(EnvironmentLightSettings targetSettings)
    {
        lightSettings.color = targetSettings.color;
        lightSettings.speed = targetSettings.speed;
        lightSettings.intensity = targetSettings.intensity;
    }
}

[System.Serializable]
public class EnvironmentLightSettings
{
    public Light light;
    public float intensity;
    public float speed;
    public Color color;
}

[System.Serializable]
public class CollectionLight
{
    public EnvironmentLightSettings[] lightSetting;
    public string name;
}
