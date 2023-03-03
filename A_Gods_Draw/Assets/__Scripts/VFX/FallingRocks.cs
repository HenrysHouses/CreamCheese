using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.Effects;


public class FallingRocks : MonoBehaviour
{
    public ParticleSystem pp;

    // Start is called before the first frame update
    void Start()
    {
        CameraEffects.ShakeOnce(5,5);
        pp = GetComponentInChildren<ParticleSystem>();
        pp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
