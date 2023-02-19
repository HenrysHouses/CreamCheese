// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCard_Behaviour : Card_Behaviour
{
    int health;
    int maxHealth;
    public int Health => health;
    GodCardAction action;
    GodPlacement godPlacement;
    protected new GodCard_ScriptableObject card_so;
    public GodCard_ScriptableObject CardSO => card_so;

    public void Initialize(GodCard_ScriptableObject card, CardElements elements)
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

    public void ApplyLevels(CardExperience experience)
    {
        stats = new CardStats();
        stats.UpgradePath.Experience.ID = experience.ID;
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        //action.OnPlay(board, card_so.strength);

        //Wait for animations, etc

        TurnController.shouldWaitForAnims = false;
        yield break;
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

    protected override void OnBeingSelected()
    {
        controller.GodPlacement.SetGod(this);
        Debug.LogWarning("Temporary fix");
        action.OnPlay(controller.GetBoard(), card_so.strength);
        //Play(controller.GetBoard());
    }

    public override void OnAction()
    {
        action.Act(controller.GetBoard(), 0);
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


    internal void Buff(ActionCard_Behaviour nonGod_Behaviour)
    {
        nonGod_Behaviour.Buff(card_so.strength, true);
    }
    internal void DeBuff(ActionCard_Behaviour nonGod_Behaviour)
    {
        nonGod_Behaviour.DeBuff(card_so.strength, true);
    }

    public override CardPlayData getCardPlayData()
    {
        CardPlayData data = new CardPlayData();
        data.CardType = card_so;
        data.Experience.ID = stats.UpgradePath.Experience.ID;
        return data;
    }
}
