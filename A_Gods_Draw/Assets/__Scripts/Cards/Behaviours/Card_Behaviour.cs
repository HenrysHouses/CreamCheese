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
    protected bool onPlayerHand = false;

    public readonly bool isReady = false;
    [field:SerializeField] public Transform ParentTransform {get; private set;}

    public Card_SO CardSO => card_so;
    public string Name => card_so.cardName;
    public TurnController Controller => controller;


    private void Awake() 
    {
        ParentTransform = transform.parent.parent;    
    }


    protected abstract void OnBeingSelected();

    protected virtual void OnPlacedInLane() { }
    protected virtual bool ReadyToBePlaced()
    {
        return true;
    }
    protected virtual IEnumerator Play(BoardStateController board)
    {
        yield return new WaitUntil(ReadyToBePlaced);
        GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
        GetComponentInParent<BoxCollider>().enabled = false;
        // manager.FinishedPlay(this);

        
        // if(this is Attack_Behaviour attack_)
        //     manager.OnDeSelectedAttackCard?.Invoke();
    }

    protected CardAction GetAction(CardActionData card)
    {
        switch (card.actionEnum)
        {
            case CardActionEnum.Attack:
                return new AttackCardAction(card.actionStrength);
            case CardActionEnum.Defence:
                return new DefendCardAction(card.actionStrength);
            case CardActionEnum.Buff:
                return new BuffCardAction(card.actionStrength, false);
            case CardActionEnum.Instakill:
                return new InstakillCardAction(card.actionStrength);
            case CardActionEnum.Chained:
                return new ChainCardAction(card.actionStrength);
            case CardActionEnum.Storr:
                return new StorrCardAction(card.actionStrength);
            case CardActionEnum.SplashDMG:
                return new SplashDMGCardAction(card.actionStrength);
            case CardActionEnum.Heal:
                return new HealCardAction(card.actionStrength);
            case CardActionEnum.Draw:
                return new DrawCardAction(card.actionStrength);
            case CardActionEnum.Weaken:
                return new WeakenCardAction(card.actionStrength);
            case CardActionEnum.Poison:
                return new PoisonCardAction(card.actionStrength);
            case CardActionEnum.HealPrevention:
                return new HealPreventionCardAction(card.actionStrength);
            case CardActionEnum.Frostbite:
                return new FrostbiteCardAction(card.actionStrength);
            default:
                return null;
        }

    }
    protected GodCardAction GetAction(GodActionEnum card)
    {
        return card switch
        {
            GodActionEnum.Tyr => new TyrActions(),
            
            GodActionEnum.Eir => new EirActions(),
            _ => null,
        };
    }


    internal virtual void OnClickOnSelected() { }


    public abstract bool CardIsReady();
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
        OnPlacedInLane();
        onPlayerHand = false;
        Debug.Log("card placed");
    }
    public virtual void CancelSelection() { controller.SetSelectedCard(); }
    public virtual bool ShouldCancelSelection() { return false; }
    public virtual bool CanBeSelected() {return onPlayerHand;  }

    public void SetController(TurnController cont)
    {
        onPlayerHand = true;
        controller = cont;        
    }
    public void OnBeingClicked()
    {
        if (onPlayerHand)
        {
            if(elements.OnClickSFX.Path != "")
                SoundPlayer.PlaySound(elements.OnClickSFX, gameObject);
            controller.SetSelectedCard(this);
            OnBeingSelected();
        }
    }
    public void ChangeStrengh(int newValue)
    {
        elements.strength.text = newValue.ToString();
    }


    //Unused stuff

    // public void SetManager(TurnManager manager)
    // {
    //     this.manager = manager;
    // }

    //public void DeSelected()
    //{
    //    if (cor != null)
    //    {
    //        StopCoroutine(cor);
    //        cor = null;
    //    }
    //    GetComponentInParent<Card_ClickGlowing>().RemoveBorder();
    //}

    // internal TurnManager GetManager()
    // {
    //     return manager;
    // }

    // public bool IsThisSelected()
    // {
    //     if(manager == null)
    //     {
    //         return false;
    //     }
    //     return manager.CurrentlySelectedCard() == this;
    // }

    // public virtual void LatePlayed(BoardStateController board) { }
}