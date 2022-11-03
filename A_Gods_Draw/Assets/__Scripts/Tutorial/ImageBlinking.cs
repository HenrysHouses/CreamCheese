//!charlie
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlinking : MonoBehaviour
{
    //? blinking effect
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    [Range(0, 1)]
    public float speed = 10f;
    Image imgComp;
    private void Awake()
    {
        imgComp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        imgComp.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }
}
