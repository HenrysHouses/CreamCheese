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

    [SerializeField] MeshRenderer Horn;
    Material Outline;
    [SerializeField] float OnSize = 0.01f;

    public EventReference endTurn_SFX;
    public ParticleSystem hornEffect;

    void Start()
    {
        Outline = Horn.materials[1];
    }

    private void OnMouseDown()
    {
        SoundPlayer.PlaySound(endTurn_SFX,gameObject);
        hornEffect.Play();
        turnEnd.Invoke();
    }

    void OnMouseOver()
    {
        setOutlineSize(OnSize);
    }

    void OnMouseExit()
    {
        setOutlineSize(0);
    }

    void setOutlineSize(float size)
    {
        if(Outline != null)
            Outline.SetFloat("_Size", size);
    }
}
