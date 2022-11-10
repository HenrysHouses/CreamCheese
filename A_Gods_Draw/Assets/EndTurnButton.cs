using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FMODUnity;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField]
    UnityEvent turnEnd;

    public EventReference endTurn_SFX;

    private void OnMouseDown()
    {
        //RuntimeManager.PlayOneShot(endTurn_SFX);
        SoundPlayer.Playsound(endTurn_SFX,gameObject);
        turnEnd.Invoke();
    }
}
