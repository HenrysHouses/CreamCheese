using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenChain : MonoBehaviour
{
    public GameObject chainPrefab;
    public int numberOfChains;
    public float spacing = 1f;
    public float chainSpeed = 10f;
    public float chainGravity = 1f;
    public float chainCollisionRadius = 0.1f;
    public float chainLifetime = 2f;
    public bool activateChainEffect = false;

    private ParticleSystem[] chains;
    private bool[] chainActive;
    private List<Vector3> positions = new List<Vector3>();
    private int currentChain = 0;

    private void Start()
    {
        chains = new ParticleSystem[numberOfChains];
        chainActive = new bool[numberOfChains];

        for (int i = 0; i < numberOfChains; i++)
        {
            GameObject chain = Instantiate(chainPrefab, transform.position, Quaternion.identity);
            chains[i] = chain.GetComponent<ParticleSystem>();
            chainActive[i] = false;
        }
    }

    private void Update()
    {
        if (activateChainEffect)
        {
            activateChainEffect = false;

            if (currentChain < numberOfChains)
            {
                Vector3 position = transform.position;

                if (currentChain > 0)
                {
                    position = positions[currentChain - 1];
                }

                chains[currentChain].transform.position = position;
                chains[currentChain].gameObject.SetActive(true);
                chains[currentChain].Play();

                positions.Add(position);
                chainActive[currentChain] = true;
                currentChain++;
            }
        }

        for (int i = 0; i < numberOfChains; i++)
        {
            if (chainActive[i])
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[chains[i].particleCount];
                int count = chains[i].GetParticles(particles);

                for (int j = 0; j < count; j++)
                {
                    if (particles[j].remainingLifetime > 0f)
                    {
                        particles[j].velocity += Vector3.down * chainGravity * Time.deltaTime;
                        particles[j].position += particles[j].velocity * Time.deltaTime;
                    }
                }

                chains[i].SetParticles(particles, count);
            }
        }
    }

    public void ActivateChains()
    {
        activateChainEffect = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Monster"))
        {
            int index = -1;

            for (int i = 0; i < numberOfChains; i++)
            {
                if (chainActive[i] && index == -1)
                {
                    index = i;
                }

                if (chainActive[i])
                {
                    if (index != i)
                    {
                        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[chains[i].particleCount];
                        int count = chains[i].GetParticles(particles);

                        for (int j = 0; j < count; j++)
                        {
                            particles[j].remainingLifetime = 0f;
                            particles[j].velocity = Vector3.zero;
                        }

                        chains[i].SetParticles(particles, count);
                        chainActive[i] = false;
                    }
                }
            }
        }
    }
}
