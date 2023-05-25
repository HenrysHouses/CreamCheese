using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lowhealthflash : MonoBehaviour
{
    public float timerSpeed;
    private float timerForFlash;
    private float timer;
    public float amount;

    public float timeuntilFlash;
    public Image image;
    public bool lowHealth, flashRedWhenHit;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lowHealth)
        {
            image.color = new Color(1, 1, 1, Mathf.PingPong(Time.time, timerSpeed));
        }



        if (flashRedWhenHit)
        {
            timer += Time.deltaTime;
            if (timer > timeuntilFlash)

            {
                Color lerpedColor = Color.Lerp(new Color(1, 1, 1, 1f), new Color(1, 1, 1, 0f), Mathf.PingPong(timerForFlash, 1f));
                timerForFlash += Time.deltaTime * timerSpeed;
                image.color = lerpedColor;
                if (timerForFlash > 0.8)
                {
                    image.color = new Color(1, 1, 1, 0);
                    timerForFlash = 0;
                    flashRedWhenHit = false;
                    timer = 0;

                }

            }


        }
    }
}
