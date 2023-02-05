using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;




public static class SoundPlayer
{   
    private static Dictionary<(EventReference, GameObject),EventInstance> eventInstances = new Dictionary<(EventReference, GameObject), EventInstance>();

    public static void PlaySound(EventReference _soundEvenet, GameObject soundGO, ParamRef parameterID = null)
    {

        // if(_soundEvenet.Path == "")
        // {

        //     Debug.Log("no sound .-.");
        //     return;

        // }

        // Debug.Log("Played sound: " + _soundEvenet.Path);
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

        // if(looping)
        // {
        //     if(pbstate != PLAYBACK_STATE.PLAYING)
        //     {
        //         temperaryEvent.start();
        //     }
        // }
        if (parameterID == null)
        {
            temperaryEvent.start();
            //Debug.Log("Soundplayed");
        }
        else
        {
          //  Debug.LogError("HealthTestName: " + parameterID.Name);
          //  Debug.LogError("HealthTestValue: " + parameterID.Value);
            temperaryEvent.setParameterByName(parameterID.Name, parameterID.Value);
            temperaryEvent.start();
        }
    }

    

    public static void StopSound(EventReference _soundEvenet, GameObject soundGO)
    {
        if(eventInstances.ContainsKey((_soundEvenet, soundGO)))
        {
            eventInstances[(_soundEvenet, soundGO)].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            // ??
        }
    }
}

        // FMOD.Studio.System system = new FMOD.Studio.System();
        // FMOD.Studio.ADVANCEDSETTINGS settings = new FMOD.Studio.ADVANCEDSETTINGS();
        // settings.cbsize = default; // DONT TOUCH
        // settings.handleinitialsize = 0; // 8192 * Sizeof(Void*), Bytes
        // settings.studioupdateperiod = 0; // 20, millisecounds
        // settings.idlesampledatapoolsize = 0; // 262144, bytes
        // settings.streamingscheduledelay = 0; //8192, samples
        // settings.commandqueuesize = 10000000; //32768 ,Bytes
        // // If this doesnt work, can set any other than CBSIZE to "Zero" for it to be used as the default
        // system.setAdvancedSettings(settings);
        //  FMOD.Studio.BUFFER_INFO buffer = new FMOD.Studio.BUFFER_INFO();
        // system.getBufferUsage(out buffer);
