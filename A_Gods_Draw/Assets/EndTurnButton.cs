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

    void Start()
    {
        Debug.Log(Horn.materials.Length);
        Outline = Horn.materials[1];
        Debug.Log(Outline);
    }

    private void OnMouseDown()
    {
        SoundPlayer.PlaySound(endTurn_SFX,gameObject);
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
