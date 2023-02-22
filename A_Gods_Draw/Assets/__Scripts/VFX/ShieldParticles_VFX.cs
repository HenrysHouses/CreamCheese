using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParticles_VFX : MonoBehaviour
{
    public ParticleSystem Sparks;
    public ParticleSystem Spawn;

    public void TriggerSparks()
    {
        if(Sparks.gameObject.activeSelf)
            Sparks.Play();
        else
            Sparks.gameObject.SetActive(true);
    } 

    public void TriggerSpawn()
    {
        if(Spawn.gameObject.activeSelf)
            Spawn.Play();
        else
            Spawn.gameObject.SetActive(true);
    } 
}
