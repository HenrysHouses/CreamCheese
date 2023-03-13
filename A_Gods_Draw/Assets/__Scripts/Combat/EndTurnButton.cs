// Written by Javier Villegas
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField]
    public UnityEvent turnEnd;
    public bool CanEndTurn = true;

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
        if(CanEndTurn)
        {
            SoundPlayer.PlaySound(endTurn_SFX,gameObject);
            hornEffect.Play();
            turnEnd.Invoke();
        }
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
