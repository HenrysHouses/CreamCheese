using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{   private Dictionary<(EventReference, GameObject),EventInstance> eventInstances;
    public static SoundManager Instance;


    // Start is called before the first frame update
    void Awake()
    {   
        if(Instance != this && Instance == null)
        Instance = this;
        eventInstances = new Dictionary<(EventReference, GameObject), EventInstance>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Playsound(EventReference _soundEvenet, GameObject soundGO, bool looping = false, ParamRef parameterID = null)
    {
        //Debug.Log(soundGO);
        EventInstance temperaryEvent;
        PLAYBACK_STATE pbstate;

        if(eventInstances.ContainsKey((_soundEvenet, soundGO)))
        {
            temperaryEvent = eventInstances[(_soundEvenet, soundGO)];
            
            
        }
        else
        {
            temperaryEvent =  RuntimeManager.CreateInstance(_soundEvenet);
            eventInstances.Add((_soundEvenet, soundGO),temperaryEvent);
        }

            temperaryEvent.getPlaybackState(out pbstate);

            if(looping)
            {

                if(pbstate != PLAYBACK_STATE.PLAYING)
                {
                    temperaryEvent.start();
                }
            }
            else if (parameterID == null)
            {
            
             temperaryEvent.start();
             //Debug.Log("Soundplayed");

            }
            else
            {
                temperaryEvent.setParameterByName(parameterID.Name, parameterID.Value);
                temperaryEvent.start();
               
            }

    }

    public void StopSound(EventReference _soundEvenet, GameObject soundGO)
    {
        if(eventInstances.ContainsKey((_soundEvenet, soundGO)))
        {
            eventInstances[(_soundEvenet, soundGO)].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        }
        else
        {

        }
    }
}
