using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_VFX : MonoBehaviour
{
    [field:SerializeField] public bool isPlaying {get; private set;}
    [SerializeField] WavePlayer ShockWaves;
    [SerializeField] ShockWavePlayer finalShockwave;
    [SerializeField] float finalShockwaveDelay = 1;
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(StartExplosion());
    }

    public void play()
    {
        StartCoroutine(StartExplosion());
    }

    public IEnumerator StartExplosion()
    {
        gameObject.SetActive(true);
        isPlaying = true;
        // yield return new WaitForEndOfFrame();
        ShockWaves.play();
        yield return new WaitForSeconds(finalShockwaveDelay);
        finalShockwave.play();
        yield return new WaitUntil(() => !finalShockwave.isPlaying);
        isPlaying = false;
        gameObject.SetActive(false);
    }
}
