//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_ClickGlowing : MonoBehaviour
{
    public Material nonGlow;
    public Material glow;

    bool nonGlowing = true;
    bool glowing = false;


    void OnMouseDown()
    {
        if (nonGlow)
        {
            gameObject.GetComponent<Renderer>().material = glow;
            nonGlowing = true;
            glowing = false;
        }
        else if (glow && glowing)
        {
            gameObject.GetComponent<Renderer>().material = nonGlow;
            nonGlowing = true;
            glowing = false;
        }
    }

}
