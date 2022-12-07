using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 orgPos;
    private float currentTime;
    private float lastFrame;
    private float faketimer;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Faketimer is for when we want to pause the game and the camera shake should still do
        currentTime = Time.realtimeSinceStartup;
        faketimer = currentTime - lastFrame;
        lastFrame = currentTime;
    }

    public static void Shake(float duration, float amount)
    {
        instance.orgPos = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = orgPos + Random.insideUnitSphere * amount;

            duration -= faketimer;

            yield return null;
        }

        transform.localPosition = orgPos;
    }
}
