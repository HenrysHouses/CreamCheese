using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticlesOnOff : MonoBehaviour
{
    public float randomTimer;
    private float timer;
    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        var main = particle.main;
        main.duration = Random.Range(2,30);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(particle.isStopped)
        {
            var main = particle.main;
            main.duration = Random.Range(2,30);
            
            particle.Play();

            //randomTimer = Random.Range(0,20);
            //timer += Time.deltaTime;
            //if(timer > randomTimer)
            //{
            //}

        }
    }
}
