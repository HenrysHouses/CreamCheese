using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class VCA_Controller : MonoBehaviour
{
    private FMOD.Studio.VCA vcaController;
    public string vcaName;

    private Slider slider;
    public EventReference SFXtestSound;
    private bool soundplayed;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        slider = GetComponent<Slider>();
    }


    void update()
    {
        if(soundplayed)
        {
            timer += Time.deltaTime;
            if(timer > 2)
            {
                timer = 0;
                soundplayed = false;
            }
        }
    }

    public void SetVolunme(float volume)
    {
        vcaController.setVolume(volume); 
        if(vcaName == "SFX")
        {
            
            SoundPlayer.PlaySound(SFXtestSound,gameObject);
                
            
        }
    }
}
