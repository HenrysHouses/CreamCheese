using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShockWavePlayer : MonoBehaviour
{
    [SerializeField] Renderer thisRenderer;
    Material thisMaterial;
    [SerializeField] float timeOffset;
    [SerializeField] ParticleSystem Particle;
    [SerializeField] float particleDelay;

    void Awake()
    {
        if(!thisRenderer)
            thisRenderer = GetComponent<Renderer>();
        thisMaterial = thisRenderer.material;
    }

    public void play(float delay = 0)
    {
        thisMaterial.SetFloat("_OffsetX", Time.realtimeSinceStartup + timeOffset+ delay);
        thisMaterial.SetFloat("_Play", 1);

        if(Particle != null)
        {
            StartCoroutine(PlayParticle(delay));
        }
    }

    IEnumerator PlayParticle(float delay)
    {
        yield return new WaitForSeconds(delay+particleDelay);
        Particle.Play();
    }
}