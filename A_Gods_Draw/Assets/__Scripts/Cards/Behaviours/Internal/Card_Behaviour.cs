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

    protected CardAction GetAction(CardActionEnum ActionType)
    {
        switch (ActionType)
        {
            case CardActionEnum.Attack:
                return new AttackCardAction();
            case CardActionEnum.Defence:
                return new DefendCardAction();
            case CardActionEnum.Buff:
                return new BuffCardAction();
            case CardActionEnum.Instakill:
                return new InstakillCardAction();
            case CardActionEnum.Chained:
                return new ChainCardAction();
            case CardActionEnum.Offering:
                return new OfferingCardAction();
            case CardActionEnum.SplashDMG:
                return new SplashDMGCardAction();
            case CardActionEnum.Heal:
                return new HealCardAction();
            case CardActionEnum.Draw:
                return new DrawCardAction();
            case CardActionEnum.Weaken:
                return new WeakenCardAction();
            case CardActionEnum.Poison:
                return new PoisonCardAction();
            case CardActionEnum.HealPrevention:
                return new HealPreventionCardAction();
            case CardActionEnum.Frostbite:
                return new FrostbiteCardAction();
            case CardActionEnum.Leach:
                return new LeachCardAction();
            case CardActionEnum.BuffAll:
                return new BuffAllCardAction();
            case CardActionEnum.Exhaust:
                return new ExhaustCardAction();
            case CardActionEnum.Earthquake:
                return new EarthquakeCardAction();
            default:
                return null;
        }

    }
    protected GodCardAction GetAction(GodActionEnum card)
    {
        switch(card)
        {
            case GodActionEnum.Tyr:
                return new TyrActions();

            case GodActionEnum.Eir:
                return new EirActions();
            
            default:
                return null;
        }
    }

    internal virtual void OnClickOnSelected() { }
    public virtual bool CardIsReady() { return true; }
    public abstract void OnAction();

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
        GainExperience();
        OnPlacedInLane();
        onPlayerHand = false;
    }


    protected virtual void GainExperience(){}
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