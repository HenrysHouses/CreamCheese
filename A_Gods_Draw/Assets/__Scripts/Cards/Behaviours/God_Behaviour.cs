using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God_Behaviour : Card_Behaviour
{
    public int maxHealth;
    int health;

    God_Card god_SO;

    Defense_Behaviour posibleDefender;

    int defendFor;
    public override void Initialize(Card_SO card)
    {
        this.card = card;
        god_SO = card as God_Card;
        maxHealth = god_SO.health;
        health = maxHealth;
    }

    public void SearchToBuff(List<NonGod_Behaviour> currentLane)
    {
        foreach (NonGod_Behaviour card in currentLane)
        {
            if (card.GetNonGod().correspondingGod == this.card.name)
            {
                card.GetBuff(true, 2f);
            }
        }
    }
    public void OnRetire(List<NonGod_Behaviour> currentLane)
    {
        foreach (NonGod_Behaviour card in currentLane)
        {
            if (card.GetNonGod().correspondingGod == this.card.name)
            {
                card.GetBuff(true, 0.5f);
            }
        }
    }

    public void DealDamage(int amount)
    {
        if (amount > defendFor)
        {
            health -= amount + defendFor;
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

    public virtual void OnTurnStart() { }

    public string GetName() { return card.cardname; }

}