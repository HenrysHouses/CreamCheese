using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrder : MonoBehaviour
{
    [SerializeField] VFXDestroyOrder[] Order;
    public bool shouldStopParticles = true, shouldStopAnimations = true;
    public void destroyVFX()
    {
        float LongestTime = 0;
        foreach(var VFX in Order)
        {
            Destroy(VFX.VFX, VFX.DestroyDelay);
        
            if(LongestTime < VFX.DestroyDelay)
                LongestTime = VFX.DestroyDelay;
        }
        Destroy(gameObject, LongestTime);
    }

    public void StopAllParticles()
    {
        if(!shouldStopParticles)
            return;
        
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        foreach (var item in particles)
        {
            item.Stop();
        }
    }

    public void StopAllAnimations()
    {
        if(!shouldStopAnimations)
            return;
        
        Animator[] animators = GetComponentsInChildren<Animator>();

        foreach (var item in animators)
        {
            // item.Play("Stop");
            item.enabled = false;
        }
    }
}

[System.Serializable]
public struct VFXDestroyOrder
{
    public GameObject VFX;
    public float DestroyDelay;
}