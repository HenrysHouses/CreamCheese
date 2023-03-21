// Written by Javier Villegas
// Modified by Henrik Hustoft

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;
using UnityEngine.Events;

/// <summary>
/// ScriptableObject containing data only necessary for god cards
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/God card"), System.Serializable]
public class GodCard_ScriptableObject : Card_SO
{
    public int strength;
    public int health;
    public GodActionEnum godAction;
    public EventReference enterBattlefield_SFX;
    public GameObject God_Model;
    [HideInInspector] public UnityEvent<string> OnDialogue;

    [SerializeField] EnemyClassNames enemyClassNames;

    public GodDialogue[] dialogues;

    /// <summary>
    /// Just to make sure the type is God
    /// </summary>
    private void OnValidate() {
        type = CardType.God;
        // getGlyphs();
    }

    private void OnEnable() {
        OnDialogue = new UnityEvent<string>();
    }

    private void OnDisable() {
        OnDialogue = new UnityEvent<string>();
    }

    // public override CardActionEnum[] getGlyphs()
    // {
    //     Debug.LogError("God icons are not implemented");
    //     return null;
    // }

    /// <summary>Instantiates a dialogue box with dialogue from the god</summary>
    /// <param name="trigger">Dialogue trigger type</param>
    /// <param name="source">Object which dialogue should react to</param>
    public void StartDialogue(GodDialogueTrigger trigger, UnityEngine.Object source = null)
    {
        Card_SO targetCard;
        GodDialogue data;

        switch(trigger)
        {
            case GodDialogueTrigger.Played:
            case GodDialogueTrigger.Draw:
            case GodDialogueTrigger.Discard:
            case GodDialogueTrigger.Dying:
                targetCard = source as Card_SO;
                data = findRelatedDialogue(trigger, targetCard);
                if(data == null)
                {
                    data = findRelatedDialogue(trigger);
                    if(data == null)
                    {
                        break;
                    }
                }
                // Debug.Log("Dialogue source: " + source + ", type: " + trigger);

                OnDialogue?.Invoke("isSpeaking");
                DialogueController.instance.SpawnDialogue(data.dialogue, Vector2.zero);
                break;
            case GodDialogueTrigger.Hurt:
            case GodDialogueTrigger.SeeEnemy:
                string damageSourceName = "None";
                
                foreach (var enemy in EnemyClassNames.instance.Names)
                {
                    if(source.ToString().Contains(enemy))
                        damageSourceName = enemy;
                }
            
                data = findRelatedDialogue(trigger, damageSourceName);
                if(data == null)
                {
                    break;
                }
                DialogueController.instance.SpawnDialogue(data.dialogue, Vector2.zero);
                Debug.Log("Dialogue source: " + damageSourceName + "(Monster), type: " + trigger);                
                OnDialogue?.Invoke("isSpeaking");
                break;
            case GodDialogueTrigger.EnemyKill:
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

    /// <summary>Finds a dialogue related to an enemy</summary>
    /// <param name="enemy">The enemy to react to</param>
    /// <returns>matching dialogue or null</returns>
    GodDialogue findRelatedDialogue(GodDialogueTrigger trigger, string enemy)
    {
        List<GodDialogue> matches = new List<GodDialogue>();

        foreach (var currDialogue in dialogues)
        {
            if(trigger != currDialogue.trigger)
                continue;

            if(enemy == EnemyClassNames.instance.Names[currDialogue.enemyTrigger])
                matches.Add(currDialogue);
        }

        if(matches.Count == 0)
        {
            // Debug.LogWarning("Found no dialogue for the trigger: " + enemy + "(IMonster)");
            return null;
        }

        int rand = UnityEngine.Random.Range(0, matches.Count);
        return matches[rand];
    }

    /// <summary>Finds a dialogue related to a card</summary>
    /// <param name="card">Target card to react to</param>
    /// <returns>matching dialogue or null</returns>
    GodDialogue findRelatedDialogue(GodDialogueTrigger trigger, Card_SO card)
    {
        List<GodDialogue> matches = new List<GodDialogue>();

        foreach (var currDialogue in dialogues)
        {
            if(currDialogue.GenericTrigger)
                continue;

            if(currDialogue.cardTrigger == null)
                continue;

            if(currDialogue.trigger != trigger)
                continue;

            if(currDialogue.cardTrigger.cardName == card.cardName)
                matches.Add(currDialogue);            
        }

        if(matches.Count == 0)
        {
            // Debug.LogWarning("Found no dialogue for the trigger: " + trigger + " by soure: " + card);
            return null;
        }

        int rand = UnityEngine.Random.Range(0, matches.Count);
        return matches[rand];
    }

    /// <summary>Finds a generic dialogue related to a trigger</summary>
    /// <param name="trigger">Type of dialogue trigger</param>
    /// <returns>A random matching dialogue or null</returns>
    GodDialogue findRelatedDialogue(GodDialogueTrigger trigger)
    {
        List<GodDialogue> matches = new List<GodDialogue>();

        foreach (var currDialogue in dialogues)
        {
            if(currDialogue.trigger == trigger && currDialogue.GenericTrigger)
                matches.Add(currDialogue);            
        }

        if(matches.Count == 0)
        {
            // Debug.LogWarning("Found no dialogue for the trigger: " + trigger);
            return null;
        }

        // Finds random dialogue and decides if it should play based on its chance
        int rand = UnityEngine.Random.Range(0, matches.Count);
        float randPlayChance = UnityEngine.Random.Range(0f, 1f);

        if(matches[rand].checkChance(randPlayChance))
            return matches[rand];
        return null;
    }
}


public static class GodColorGetter
{
    private static GodColor[] GodColors = new GodColor[]
    {
        new GodColor(GodActionEnum.Eir, new Color(0f,1.31950796f,1.15392268f,1f), new Color(1.082353f,1.49803925f,1.01176476f,1f)),
        new GodColor(GodActionEnum.Tyr, new Color(0.936977565f,0f,3.44159079f,1f), new Color(1.62502766f,2.21222258f,2.51264787f,1f))
    };

    public static GodColor find(GodActionEnum God)
    {
        for (int i = 0; i < GodColorGetter.GodColors.Length; i++)
        {
            if(GodColorGetter.GodColors[i].God == God)
                return GodColorGetter.GodColors[i];
        }
        return GodColorGetter.GodColors[0];
    }
}

public struct GodColor
{
    public GodColor(GodActionEnum God, Color Main, Color Secondary)
    {
        this.God = God;
        this.MainColor = Main;
        this.SecondaryColor = Secondary;
    }

    public GodActionEnum God;
    [ColorUsage(true, true)]
    public Color MainColor, SecondaryColor;
}
