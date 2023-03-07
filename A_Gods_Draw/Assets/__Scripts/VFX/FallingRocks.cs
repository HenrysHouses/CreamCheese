using UnityEngine;
using DitzeGames.Effects;

public class FallingRocks : MonoBehaviour
{
    public ParticleSystem stoneParticles;
    public ParticleSystem dustParticles;
    public GameObject stones;

    private  ParticleSystem.Particle[] particles;
    ParticleSystem.EmissionModule emission;
    private int count;
    public float intensity = 1.0f;

    void Start()
    {
        stones.SetActive(false);
        emission = stoneParticles.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(40 * intensity);

        ParticleSystem.ShapeModule shape = stoneParticles.shape;
        shape.angle = 30;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color32(100, 100, 100, 255), 0.0f), new GradientColorKey(new Color32(50, 50, 50, 255), 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = stoneParticles.colorOverLifetime;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = stoneParticles.sizeOverLifetime;
        // sizeOverLifetime.sizeMultiplier = new ParticleSystem.MinMaxCurve(0.0f, new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0) }));

        particles = new ParticleSystem.Particle[20];
        count = stoneParticles.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            particles[i].startColor = new Color32(255, 255, 255, 255);
            particles[i].startSize = 0.05f;
            particles[i].velocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1.0f, 2.0f), Random.Range(-0.5f, 0.5f));
            particles[i].angularVelocity = Random.Range(-90.0f, 90.0f);
            particles[i].startLifetime = 5.0f;
        }

        stoneParticles.SetParticles(particles, count);

    }

    private void Update()
    {


    }

    public void Crumble(float camshakeLength, float camShakeIntensity, float rockfallIntensity)
    {
        stones.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(40 * intensity);
            particles[i].startColor = new Color32(255, 255, 255, 255);
            particles[i].startSize = 0.05f;
            particles[i].velocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1.0f, 2.0f), Random.Range(-0.5f, 0.5f));
            particles[i].angularVelocity = Random.Range(-90.0f, 90.0f);
            particles[i].startLifetime = 5.0f;
        }

        stoneParticles.SetParticles(particles, count);
        CameraEffects.ShakeOnce(camshakeLength,camShakeIntensity);
        stones.SetActive(false);

    }


}
