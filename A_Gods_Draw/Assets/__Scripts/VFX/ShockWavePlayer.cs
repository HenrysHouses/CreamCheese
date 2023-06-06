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
    [SerializeField] float waveDuration;
    [field:SerializeField] public bool isPlaying {get; private set;}

    void Awake()
    {
        if(!thisRenderer)
            thisRenderer = GetComponent<Renderer>();
        thisMaterial = thisRenderer.material;
    }

    public void play(float delay = 0)
    {
        isPlaying = true;
        thisMaterial.SetFloat("_OffsetX", Time.realtimeSinceStartup + timeOffset+ delay);
        thisMaterial.SetFloat("_Play", 1);

        StartCoroutine(waitForCompletion(delay));
    }

    IEnumerator waitForCompletion(float waveDelay)
    {
        if(Particle != null)
        {
            yield return new WaitForSeconds(waveDelay+particleDelay);
            Particle.Play();

            yield return new WaitUntil(() => Particle.particleCount == 0);
        }
        else
            yield return new WaitForSeconds(waveDuration);
        isPlaying = false;
    }
}