// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen,
//  Charlie Eikaas

using System.Collections;
using UnityEngine;

public abstract class Card_Behaviour : BoardElement
{
    protected TurnController controller;
    protected Card_SO card_so;
    protected CardElements elements;
    public CardStats stats;
    protected bool onPlayerHand = false;
    [field:SerializeField] public Transform ParentTransform {get; private set;}

    // public Card_SO CardSO => card_so;
    // public string Name => card_so.cardName;
    public TurnController Controller => controller;


    private void Awake() 
    {
        ParentTransform = transform.parent.parent;    
    }

    public abstract CardPlayData getCardPlayData();
    protected abstract void OnBeingSelected();
    protected virtual void OnPlacedInLane() {}
    protected virtual IEnumerator Play(BoardStateController board)
    {
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
        GetComponentInParent<BoxCollider>().enabled = false;
        yield break;
    }

    internal virtual void OnClickOnSelected() { }
    public virtual bool CardIsReady() { return true; }
    public abstract void OnAction();

    public virtual void MissClick() => CancelSelection();

    public void Placed()
    {
        // resizing the collider when exiting the player hand
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 center = collider.center;
        center.y = -0.0007099053f;
        Vector3 size = collider.size;
        size.y = 0.2012218f;
        collider.size = size;
        collider.center = center; // centre -0.0007099053 // size 0.2012218
        // transform.GetComponent<BoxCollider>().enabled = false;
        OnPlacedInLane();
        onPlayerHand = false;
    }


    public virtual void GainExperience(){}
    public virtual void CancelSelection() 
    { 
        controller.SetSelectedCard(); 
        CameraMovement.instance.SetCameraView(CameraView.Reset);
    }
    public virtual bool ShouldCancelSelection() { return false; }
    public virtual bool CanBeSelected() { return onPlayerHand; }

    public void SetController(TurnController cont)
    {
        onPlayerHand = true;
        controller = cont;        
    }
    public void OnBeingClicked()
    {
        if (onPlayerHand)
        {
            SoundPlayer.PlaySound(elements.OnClickSFX, gameObject);
            controller.SetSelectedCard(this);
            OnBeingSelected();
        }
    }
}