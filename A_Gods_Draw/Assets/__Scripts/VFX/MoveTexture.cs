using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTexture : MonoBehaviour
{
    public float scrollX, scrollY;


    // Update is called once per frame
    void Update()
    {
        ScrollTextureStart();
        
    }

    public void ScrollTextureStart()
    {
        float ofsettX = Time.time * scrollX;
        float ofsettY = Time.time * scrollY;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(ofsettX,ofsettY);


    }
}
