// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God_Behaviour : Card_Behaviour
{
    int health;
    int maxHealth;

    public int Health => health;

    GodCardAction action;
    GodPlacement godPlacement;

    int defendFor;

    protected new God_Card_SO card_so;
    public new God_Card_SO CardSO => card_so;

    public void Initialize(God_Card_SO card, CardElements elements)
    {
        this.card_so = card;
        maxHealth = card.health;
        health = maxHealth;

        action = GetAction(card.godAction);

        this.elements = elements;
    }


    public void SetPlace(GodPlacement place)
    {
        godPlacement = place;
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in board.placedCards)
        {
            if (card.CardSO.correspondingGod == card_so.godAction)
            {
                card.Buff(card_so.strengh, true);
            }
        }

        action.OnPlay(board);

        //Wait for animations, etc
        yield return new WaitUntil(() => true /* action.IsReady() */);

        TurnController.shouldWaitForAnims = false;
    }
    public void OnRetire(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in board.placedCards)
        {
            if (card.CardSO.correspondingGod == card_so.godAction)
            {
                card.DeBuff(card_so.strengh, true);
            }
        }
    }

    public void DealDamage(int amount, UnityEngine.Object source)
    {
        health -= amount;

        if(health > 0)
            card_so.StartDialogue(GodDialogueTrigger.Hurt, source);

        godPlacement.UpdateUI();

        if (health <= 0)
        {
            health = 0;

            godPlacement.UpdateUI();
            godPlacement.RemoveGod();
            controller.GodDied(this);
        }
    }

    //public void CanBeDefendedBy(Defense_Behaviour defense_Behaviour)
    //{
    //    posibleDefender = defense_Behaviour;
    //}

    //private void OnMouseDown()
    //{
    //    if (posibleDefender)
    //    {
    //        posibleDefender.ItDefends(null, this);
    //    }
    //    posibleDefender = null;
    //}

    //public void Defend(int amount)
    //{
    //    defendFor += amount;
    //}

    private void OnMouseOver()
    {
        if (onPlayerHand)
            godPlacement.godArrow.color = Color.magenta;
    }

    private void OnMouseExit()
    {
        if (onPlayerHand)
            godPlacement.godArrow.color = Color.white;
    }
    //public int GetStrengh()
    //{
    //    return card_so.strengh;
    //}

    //public virtual void OnTurnStart() { }

    protected override void OnBeingSelected()
    {
        controller.GodPlacement.SetGod(this);
        StartCoroutine(Play(controller.GetBoard()));
    }

    public override void OnAction()
    {
        action.Act(controller.GetBoard(), 0);
    }

    public override bool CardIsReady()
    {
        return true;
    }

    public void StartDialogue(GodDialogueTrigger trigger, UnityEngine.Object source = null)
    {
        Card_SO targetCard;
        GodDialogue data;

        switch(trigger)
        {
            case GodDialogueTrigger.Played:
            case GodDialogueTrigger.Draw:
            case GodDialogueTrigger.Discard:
                targetCard = source as Card_SO;
                data = findRelatedDialogue(trigger, targetCard);
                if(data == null)
                {
                    data = findRelatedDialogue(trigger);
                    if(data == null)
                        break;
                }
                Debug.Log("Dialogue source: " + source);
                DialogueController.instance.SpawnDialogue(data.dialogue, Vector2.zero);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>Finds a dialogue related to a board element</summary>
    /// <param name="target">The board element to react to</param>
    /// <returns>matching dialogue or null</returns>
    GodDialogue findRelatedDialogue<T>(T target) where T : BoardElement
    {
        throw new NotImplementedException();
    }

    /// <summary>Finds a dialogue related to a card</summary>
    /// <param name="card">Target card to react to</param>
    /// <returns>matching dialogue or null</returns>
    GodDialogue findRelatedDialogue(GodDialogueTrigger trigger, Card_SO card)
    {
        List<GodDialogue> matches = new List<GodDialogue>();

        foreach (var dialogue in card_so.dialogues)
        {
            if(dialogue.GenericTrigger)
                continue;

            if(dialogue.cardTrigger == null)
                continue;

            if(dialogue.trigger != trigger)
                continue;

            if(dialogue.cardTrigger.cardName == card.cardName)
                matches.Add(dialogue);            
        }

        if(matches.Count == 0)
            return null;

        int rand = UnityEngine.Random.Range(0, matches.Count);
        return matches[rand];
    }

    /// <summary>Finds a generic dialogue related to a trigger</summary>
    /// <param name="trigger">Type of dialogue trigger</param>
    /// <returns>A random matching dialogue or null</returns>
    GodDialogue findRelatedDialogue(GodDialogueTrigger trigger)
    {
        List<GodDialogue> matches = new List<GodDialogue>();

        foreach (var dialogue in card_so.dialogues)
        {
            if(dialogue.trigger == trigger && dialogue.GenericTrigger)
                matches.Add(dialogue);            
        }

        if(matches.Count == 0)
            return null;

        // Finds random dialogue and decides if it should play based on its chance
        int rand = UnityEngine.Random.Range(0, matches.Count);
        float randPlayChance = UnityEngine.Random.Range(0f, 1f);

        if(matches[rand].checkChance(randPlayChance))
            return matches[rand];
        return null;
    }


    internal void Buff(NonGod_Behaviour nonGod_Behaviour)
    {
        nonGod_Behaviour.Buff(card_so.strengh, true);
        nonGod_Behaviour.BuffedByGod();
    }
    internal void DeBuff(NonGod_Behaviour nonGod_Behaviour)
    {
        nonGod_Behaviour.DeBuff(card_so.strengh, true);
    }
}
