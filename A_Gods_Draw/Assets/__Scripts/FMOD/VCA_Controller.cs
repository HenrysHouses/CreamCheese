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

    // Start is called before the first frame update
    void Start()
    {
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        slider = GetComponent<Slider>();
    }

    public void SetVolunme(float volume)
    {
        vcaController.setVolume(volume);
    }
}
