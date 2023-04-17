using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class ThunderCloud : MonoBehaviour
{
    private ParticleSystem particle;
    public ParticleSystem rain;
    private float timer;
    private float randromTimer;
    public EventReference thunderSFX;

    void Start()
    {
        SoundPlayer.PlaySound(thunderSFX,gameObject);
        particle = GetComponent<ParticleSystem>();
        rain = GetComponentInChildren<ParticleSystem>();
        var main = particle.main;
        var rainMain = rain.main;
       // main.duration = Random.Range(20, 50);
       // rainMain.duration = main.duration - 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(rain.isStopped || rain.isPaused)
        {
            SoundPlayer.StopSound(thunderSFX,gameObject);
        }
        if (particle.isStopped)
        {
            timer += Time.deltaTime;
            randromTimer = Random.Range(30, 100);
            if (timer > randromTimer)
            {
                var main = particle.main;
                var rainMain = rain.main;
                main.duration = Random.Range(20,50);
                rainMain.duration = main.duration;
                particle.Play();
                rain.Play();
                SoundPlayer.PlaySound(thunderSFX,gameObject);
                

            }
        }


    }
}
