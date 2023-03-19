using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HoverOverMenuThings : MonoBehaviour
{
    private Animator anim;
    public EventReference sound;
    private bool playedSound;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   private void OnMouseOver()
    {
        anim.SetBool("Hover",true);
        if(!playedSound)
        {
            SoundPlayer.PlaySound(sound,gameObject);
            playedSound = true;

        }
    }

    private void OnMouseExit()
    {
        anim.SetBool("Hover", false);  
        playedSound = false;  
    }
}
