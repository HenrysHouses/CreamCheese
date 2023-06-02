using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_VFX : MonoBehaviour
{
    [SerializeField] WavePlayer ShockWaves;
    [SerializeField] ShockWavePlayer finalShockwave;
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(StartExplosion());
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForEndOfFrame();
        ShockWaves.play();
        yield return new WaitUntil(() => !ShockWaves.isPlaying);
        finalShockwave.play();
    }
}
