// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>
/// Data that each card action will need
/// </summary>
[System.Serializable]
public struct CardActionData
{
    public CardActionEnum actionEnum;
    public int actionStrength;
    public bool PlayOnPlacedOrTriggered_SFX;
    public EventReference action_SFX;
    public ActionVFX _VFX;
}


/// <summary>
/// Contains an array of actions for a number of targets,
/// and methods so that it can be treated as an array
/// </summary>
[System.Serializable]
public struct ActionsForTarget
{
    public int numOfTargets;
    public List<CardActionData> targetActions;

    public void SetNTar(int a)
    {
        numOfTargets = a;
    }
    public CardActionData this[int key]
    {
        get => targetActions[key];
        set => targetActions[key] = value;
    }
    public int Count => targetActions.Count;
}


/// <summary>
/// ScriptableObject containing data only necessary for non-god cards
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card"), System.Serializable]
public class NonGod_Card_SO : Card_SO
{
    [HideInInspector] public int cardStrenghIndex;
    public List<ActionsForTarget> targetActions = new();
    public List<ActionsForTarget> onGodBuff = new();
    public GodActionEnum correspondingGod;

    /// <summary>
    /// Used to calculate from where should the card loader get the number of the strengh
    /// </summary>
    private void OnValidate()
    {
        cardStrenghIndex = 0;
        if (targetActions.Count == 0)
            return;

        foreach (ActionsForTarget a in targetActions)
        {
            a.SetNTar(1);
        }

        if (type == CardType.Attack)
        {
            while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Attack)
            {
                if (cardStrenghIndex == targetActions.Count - 1)
                    break;
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Defence)
        {
            while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Defend)
            {
                if (cardStrenghIndex == targetActions.Count - 1)
                    break;
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Buff)
        {
            while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Buff)
            {
                if (cardStrenghIndex == targetActions.Count - 1)
                    break;
                cardStrenghIndex++;
            }
        }
        else
        {
            cardStrenghIndex = 0;
        }
    }
}
