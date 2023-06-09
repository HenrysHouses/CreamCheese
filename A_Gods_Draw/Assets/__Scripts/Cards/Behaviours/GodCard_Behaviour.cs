// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCard_Behaviour : Card_Behaviour , IMonsterTarget
{
    int health;
    int maxHealth;
    private float outlineSize;
    private Renderer[] renderers;
    public int Health => health;
    GodCardAction action;
    GodPlacement godPlacement;
    protected new GodCard_ScriptableObject card_so;
    public GodCard_ScriptableObject CardSO => card_so;
    public Animator animator;
    private List<Monster> targettedBy;
    private Coroutine OnSelectedRoutine;
    private GodConfirmation GodSlot;

    public void Initialize(GodCard_ScriptableObject card, CardElements elements)
    {
        this.card_so = card;
        maxHealth = card.health;
        health = maxHealth;

        action = GodCardAction.GetAction(card.godAction);

        this.elements = elements;
        Material GlowMat = elements.Glow.GetComponentInChildren<Renderer>().material;
        GodColor color = GodColorGetter.find(card.godAction);
        GlowMat.SetColor("_MainColor", color.MainColor);
        GlowMat.SetColor("_SecondColor", color.SecondaryColor);
        targettedBy = new List<Monster>();
        renderers = gameObject.GetComponentsInChildren<Renderer>();
        outlineSize = 0.26f;
    }


    public void SetPlace(GodPlacement place)
    {
        godPlacement = place;
    }

    public void ApplyLevels(CardExperience experience)
    {
        stats = new CardStats();
        CardExperience _XP = new CardExperience(0,0,experience.ID);
        stats.UpgradePath.Experience = _XP;
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
        {
            card_so.StartDialogue(GodDialogueTrigger.Hurt, source);
        }

        if (health <= 0)
        {
            health = 0;

            if(godPlacement.GodIsEquals(this))
            {
                godPlacement.UpdateUI();
                godPlacement.RemoveGod();
            }
            for(int i = 0; i < targettedBy.Count; i++)
                targettedBy[i].ReSelectTargets(controller.GetBoard());
            controller.GodDied(this);
            animator.SetTrigger("Die");
            return;
        }
        godPlacement.UpdateUI();
        animator.SetTrigger("TakingDamage");
    }

    public override void MissClick()
    {
        if(!gameObject)
            return;
        StopAllCoroutines();
        OnSelectedRoutine = null;
        GodSlot = null;
        ChangeCursor.instance.DefaultCursor();
    }

    public override bool ShouldCancelSelection()
    {
        if(OnSelectedRoutine == null && GodSlot == null)
            return true;
        return false;    
    }

    public override bool CardIsReady()
    {
        if(GodSlot)
            return true;
        return false;
    }

    internal override void OnClickOnSelected()
    {
        base.OnClickOnSelected();
        
        GodSlot = TurnController.PlayerClick() as GodConfirmation;

        if(GodSlot == null)
        {
            MissClick();
            return;
        }
        // if(!GodSlot.wasClicked)
        //     MissClick();
    }

    private IEnumerator ConfirmingGodPlacement()
    {
        CameraMovement.instance.SetCameraView(CameraView.BoardTopView, true);
        
        yield return new WaitUntil(() => GodSlot != null);

        if(GodSlot is GodConfirmation)
        {
            CameraMovement.instance.SetCameraView(CameraView.Reset);
            TurnController.shouldWaitForAnims = false;

            // places god
            controller.GodPlacement.SetGod(this);
            foreach (ActionCard_Behaviour _card in controller.GetBoard().allPlayedCards)
                _card.UpdateQueuedDamage();

            action.OnPlay(controller.GetBoard(), card_so.strength);
            ChangeCursor.instance.DefaultCursor();
            CameraMovement.instance.SetCameraView(CameraView.Reset);
        }
        else
            MissClick();
    }
    
    protected override void OnBeingSelected()
    {
        if (OnSelectedRoutine == null)
        {
            TurnController.shouldWaitForAnims = true;
            OnSelectedRoutine = StartCoroutine(ConfirmingGodPlacement());

            //change cursor type
            ChangeCursor.instance.GodCursor();
        }
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
                DialogueController.instance.SpawnDialogueUI(data.dialogue, Vector2.zero);
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

        CardExperience _XP = new CardExperience(stats.UpgradePath.Experience.XP, stats.UpgradePath.Experience.Level, stats.UpgradePath.Experience.ID);
        data.Experience = _XP;
        return data;
    }

    public void Targeted(GameObject _sourceGO = null)
    {

        foreach (Renderer _renderer in renderers)
        {
            if(_renderer.materials.Length > 1)
            {
                _renderer.materials[1].SetFloat("_Size", outlineSize);            
                _renderer.materials[1].SetColor("_Color", Color.red);            
            }
        }
        
    }

    public void UnTargeted(GameObject _sourceGO = null)
    {

        foreach (Renderer _renderer in renderers)
        {
            if(_renderer == null)
                continue;
            
            if(_renderer.materials.Length > 1)
            {
                _renderer.materials[1].SetFloat("_Size", 0);            
                _renderer.materials[1].SetColor("_Color", Color.red);            
            }
        }
        
    }

    public Transform GetTransform() => transform;
}
