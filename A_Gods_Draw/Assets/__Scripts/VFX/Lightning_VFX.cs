using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Lightning_VFX : MonoBehaviour
{
    public GameObject[] lightning;
    private Light lights;
    public float minFlickerTime = 0.2f;
    public float maxFlickerTime = 0.4f;
    private float TimeforLightning;
    private float timer;

    public bool startFlicker;
    private bool playedsound;
    public EventReference thunder, startOfCloudAandRain;

    // Start is called before the first frame update
    void Start()
    {


        for (int i = 0; i < lightning.Length; i++)
        {
            lights = lightning[i].GetComponent<Light>();
            // lights.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            // startFlicker = true;
            if(!playedsound)
            {
             SoundPlayer.PlaySound(startOfCloudAandRain, gameObject);
             playedsound = true;

            }
        


        }



        if (startFlicker)
        {
            TimeforLightning = Random.Range(1, 3);
            if (!playedsound)
            {
                SoundPlayer.PlaySound(thunder, gameObject);
                playedsound = true;
                timer = 0;

            }

            StartCoroutine(Flicker(lights));
            timer += Time.deltaTime;

            if (timer > TimeforLightning)
            {
                timer = 0;
                startFlicker = false;
                playedsound = false;
            }

        }
        else
        {
            StopCoroutine(Flicker(lights));
        }




    }

    IEnumerator Flicker(Light lights)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
            lights.enabled = !lights.enabled;
        }
    }
}
