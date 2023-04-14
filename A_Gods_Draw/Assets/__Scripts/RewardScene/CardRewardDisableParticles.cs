//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardDisableParticles : MonoBehaviour
{
    public List<Transform> cardObjects;
    [SerializeField] GameObject particle;
    [SerializeField] LayerMask layer;

    bool isClicked;
    bool pressedThisFrame;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            pressedThisFrame = false;
        }
        if (Input.GetMouseButtonDown(0) && !pressedThisFrame)
        {
            pressedThisFrame = true;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int _layerMask = LayerMask.NameToLayer("Card");

            if(isClicked)
            {
                foreach (Transform cards in cardObjects)
                {
                    EnableParticleSystems(cards);
                }
                isClicked = false;
            }
            else if (Physics.Raycast(ray, out hit, _layerMask))
            {
                // GameObject obj = hit.collider.GetComponentInChildren<CardReward_PopupHold>().gameObject;
                
                // foreach (Transform otherObj in cardObjects)
                // {
                //     if (otherObj.GetChild(0).gameObject != obj)
                //     {
                //         DisableParticleSystems(otherObj);
                //     }
                // }

                // // Set isClicked to true for the clicked card
                // EnableParticleSystems(obj.transform);
                isClicked = true;
            }
        }
    }

    private void DisableParticleSystems(Transform target)
    {
        ParticleSystem[] particles = target.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Pause();
            particles[i].Clear();
        }
    }
    private void EnableParticleSystems(Transform target)
    {
        ParticleSystem[] particles = target.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }
}