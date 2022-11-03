using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMOD_DrawingCards : MonoBehaviour
{
     [SerializeField] StudioEventEmitter cardflip, cardDraw, cardDrawn;

    // Start is called before the first frame update
    void Start()
    {
        cardDraw = GetComponent<StudioEventEmitter>();
        cardflip = GetComponent<StudioEventEmitter>();
        cardDrawn = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
  
    }
}
