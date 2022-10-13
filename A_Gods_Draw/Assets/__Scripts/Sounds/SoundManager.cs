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
 
        FMOD.Studio.System system = new FMOD.Studio.System();
        FMOD.Studio.ADVANCEDSETTINGS settings = new FMOD.Studio.ADVANCEDSETTINGS();
        // settings.cbsize = default; // DONT TOUCH
        settings.handleinitialsize = 0; // 8192 * Sizeof(Void*), Bytes
        settings.studioupdateperiod = 0; // 20, millisecounds
        settings.idlesampledatapoolsize = 0; // 262144, bytes
        settings.streamingscheduledelay = 0; //8192, samples
        settings.commandqueuesize = 10000000; //32768 ,Bytes
        // If this doesnt work, can set any other than CBSIZE to "Zero" for it to be used as the default


        system.setAdvancedSettings(settings);


      //  FMOD.Studio.BUFFER_INFO buffer = new FMOD.Studio.BUFFER_INFO();
        

         // system.getBufferUsage(out buffer);
        
        
        
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
