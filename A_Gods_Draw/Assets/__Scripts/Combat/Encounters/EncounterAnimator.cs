/* 
 * Written by 
 * Henrik
*/

using UnityEngine;
using HH.MultiSceneTools.Examples;

/// <summary>
/// Animator Script which animates the beginning of combat
/// </summary>
public class EncounterAnimator : MonoBehaviour
{
    [SerializeField] GameObject Particles;
    [SerializeField] Transform[] AnimateTransforms;
    [SerializeField] SceneTransition BoardTransition;
    [SerializeField] float speed = 1;

    float totalOffset;
    [SerializeField] float startOffset;
    

    void Start()
    {
        foreach (var anim in AnimateTransforms)
        {
            anim.position = anim.position - new Vector3(0, startOffset, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!BoardTransition.isTransitioning)
        {
            if(Particles)
                Particles.SetActive(true);
            AnimateTransforms[1].gameObject.SetActive(true);
            Destroy(Particles, Particles.GetComponent<ParticleSystem>().main.duration+3);
            Light light = Particles.GetComponentInChildren<Light>();

            for (int i = 0; i < AnimateTransforms.Length; i++)
            {
                Vector3 pos = AnimateTransforms[i].position;
                float offset = Time.deltaTime * speed;
                pos.y = pos.y + offset;
                AnimateTransforms[i].position = pos;

                if(i != AnimateTransforms.Length-1)
                    continue;

                totalOffset += offset;

                light.intensity -= Time.deltaTime * 1.5f;

                if(totalOffset >= startOffset)
                {
                    Destroy(this);
                }
            }
        }
    }
}
