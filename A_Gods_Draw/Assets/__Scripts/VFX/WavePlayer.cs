using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePlayer : MonoBehaviour
{
    public bool isPlaying;
    public float duration;
    public ShockWavePlayer[] Waves;
    public float waveDelay = 0;

    bool playOnStart = true;
    

    // Start is called before the first frame update
    void Start()
    {
        if(Waves.Length == 0)
            Waves = GetComponentsInChildren<ShockWavePlayer>();
        // if(playOnStart)
        //     play();
    }

    public void play()
    {
        for (int i = 0; i < Waves.Length; i++)
        {
            Waves[i].play(i * waveDelay);
        }
        isPlaying = true;
        StartCoroutine(FinishPlaying());
    }

    IEnumerator FinishPlaying()
    {
        yield return new WaitForSeconds(duration);
        isPlaying = false;
    }
}

