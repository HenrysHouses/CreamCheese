using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PressButton_SFX : MonoBehaviour
{
    public EventReference buttonSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PressButtonSFX()
    {
        SoundPlayer.PlaySound(buttonSFX,gameObject);

    }
}
