using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lowhealthflash : MonoBehaviour
{
    public float timerSpeed;
    private float timerForFlash;
    public float amount;
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

            Color lerpedColor = Color.Lerp(new Color(1, 1, 1, 1f), new Color(1, 1, 1, 0f), Mathf.PingPong(timerForFlash, 1f));
            timerForFlash += Time.deltaTime;
            image.color = lerpedColor;
            if (timerForFlash > 1)
            {
                image.color = new Color(1, 1, 1, 0);
                timerForFlash = 0;
                flashRedWhenHit = false;

            }

        }
    }
}
