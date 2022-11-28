//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashParticles : MonoBehaviour
{
    public ParticleSystem slash;
    public Transform card;

    public void Slashing()
    {
        ParticleSystem slashInstance = Instantiate(slash);
        Destroy(slashInstance, 1); //can change the time to something that feels natural
    }
}
