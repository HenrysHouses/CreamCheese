using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class God_Behaviour : Card_Behaviour
{
    int maxHealth;
    [SerializeField]
    int health;

    IGodAction action;

    God_Card god_SO;
    GodPlacement godPlacement;
    Defense_Behaviour posibleDefender;

    int defendFor;
    public override void Initialize(Card_SO card)
    {
        this.card_abs = card;
        god_SO = card as God_Card;
        maxHealth = god_SO.health;
        health = maxHealth;

        action = GetComponent<IGodAction>();
    }

    public void SetPlace(GodPlacement place)
    {
        godPlacement = place;
    }

    protected override IEnumerator Play(BoardState board)
    {
        action.OnPlay(board);

        //Wait for animations, etc
        return base.Play(board);
    }

    public override void LatePlayed(BoardState board)
    {
        gameObject.AddComponent<BoxCollider>();
        foreach (NonGod_Behaviour card in board.lane)
        {
            if (card.GetNonGod().correspondingGod == this.card_abs.name)
            {
                card.GetBuff(true, 2f);
            }
        }
    }
    public void OnRetire(List<NonGod_Behaviour> currentLane)
    {
        foreach (NonGod_Behaviour card in currentLane)
        {
            if (card.GetNonGod().correspondingGod == this.card_abs.name)
            {
                card.GetBuff(true, 0.5f);
            }
        }
    }

    public void DealDamage(int amount)
    {
        //Debug.Log("God damaged, defended for: " + defendFor);

        if (amount > defendFor)
        {
            health -= amount + defendFor;
            defendFor = 0;
        }
        else
        {
            defendFor -= amount;
        }

        //Debug.Log("God damaged, health left: " + health);

        if (health <= 0)
        {
            health = 0;
            manager.GodDied();
        }
    }

    public void CanBeDefendedBy(Defense_Behaviour defense_Behaviour)
    {
        posibleDefender = defense_Behaviour;
    }

    private void OnMouseDown()
    {
        if (posibleDefender)
        {
            posibleDefender.ItDefends(null, this);
        }
        posibleDefender = null;
    }

    public void Defend(int amount)
    {
        defendFor += amount;
    }

    internal void Buff(NonGod_Behaviour nonGod_Behaviour)
    {
        nonGod_Behaviour.GetBuff(true, 2f);
    }

    private void OnMouseOver()
    {
        /*?if (godPlacement)
        {
            godPlacement.godArrow.color = Color.magenta;
        }*/
        godPlacement.godArrow.color = Color.magenta;
    }

    private void OnMouseExit()
    {
        godPlacement.godArrow.color = Color.white;
    }

    public virtual void OnTurnStart() { }

    public string GetName() { return card_abs.cardname; }

}
