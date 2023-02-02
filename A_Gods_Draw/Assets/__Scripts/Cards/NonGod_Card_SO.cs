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
    public EventReference action_SFX;
    public ActionVFX _VFX;
}


/// <summary>
/// Contains an array of actions for a number of targets,
/// and methods so that it can be treated as an array
/// </summary>
// [System.Serializable]
// public struct ActionsForTarget
// {

//     public CardActionData this[int key]
//     {
//         get => targetActions[key];
//         set => targetActions[key] = value;
//     }
//     public int Count => targetActions.Count;
// }

[System.Serializable]
public struct ActionGroup
{
    public List<CardActionData> actionStats;
    public List<CardAction> actions;

    internal void Add(CardAction act)
    {
        actions.Add(act);
    }
}

[System.Serializable]
public class CardStats
{
    public int strength;
    public int numberOfTargets;
    public ActionGroup actionGroup;
    public GodActionEnum correspondingGod;
    public ActionGroup godBuffActions;
}

/// <summary>
/// ScriptableObject containing data only necessary for non-god cards
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card"), System.Serializable]
public class NonGod_Card_SO : Card_SO
{
    public CardStats cardStats;

    /// <summary>
    /// Used to calculate from where should the card loader get the number of the strengh
    /// </summary>
    private void OnValidate()
    {
        // cardStrenghIndex = 0;
        // if (_glyphs.numberOfTargets == 0)
        //     return;

        // if (type == CardType.Attack)
        // {
        //     while (_glyphs.actions[cardStrenghIndex].actionEnum != CardActionEnum.Attack)
        //     {
        //         if (cardStrenghIndex == targetActions.Count - 1)
        //             break;
        //         cardStrenghIndex++;
        //     }
        // }
        // else if (type == CardType.Defence)
        // {
        //     while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Defence)
        //     {
        //         if (cardStrenghIndex == targetActions.Count - 1)
        //             break;
        //         cardStrenghIndex++;
        //     }
        // }
        // else if (type == CardType.Buff)
        // {
        //     while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Buff)
        //     {
        //         if (cardStrenghIndex == targetActions.Count - 1)
        //             break;
        //         cardStrenghIndex++;
        //     }
        // }
        // else
        // {
        //     cardStrenghIndex = 0;
        // }
    }

    public override CardActionEnum[] getGlyphs()
    {
        List<CardActionEnum> allGlyphs = new List<CardActionEnum>();
        for (int i = 0; i < cardStats.actionGroup.actions.Count; i++)
        {
            foreach (var _action in cardStats.actionGroup.actionStats)
            {
                if(_action.actionEnum.ToString() == type.ToString())
                    continue;

                allGlyphs.Add(_action.actionEnum);
            }
        }
        return allGlyphs.ToArray();
    }
}