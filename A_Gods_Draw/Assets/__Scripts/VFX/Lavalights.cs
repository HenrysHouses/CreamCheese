using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavalights : MonoBehaviour
{
    public float howFastTurnOn, howfasttoturnOff, timerforLights;
    private float lighttime;
    private Light light;
    public LavaController lava;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.intensity = 0;
        
        

        

    }

    // Update is called once per frame
    void Update()
    {
        
        if(lava.turnOnLava)
        {
            LightGoesOnSlow();
        }
        if(!lava.turnOnLava)
        {
            lighttime = 0;
        }


    }

    public void LightGoesOnSlow()
    {
        
        light.intensity += howFastTurnOn * Time.deltaTime;
        if (light.intensity >= 20)
            light.intensity = 20;

        lighttime += Time.deltaTime;
        if (lighttime > timerforLights)
        {
            light.intensity -= howfasttoturnOff * Time.deltaTime;
        }



    }
}
