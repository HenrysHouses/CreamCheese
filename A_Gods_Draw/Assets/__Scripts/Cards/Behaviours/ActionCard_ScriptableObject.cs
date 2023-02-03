// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


/// <summary>
/// ScriptableObject containing data only necessary for non-god cards
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card"), System.Serializable]
public class ActionCard_ScriptableObject : Card_SO
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

/// <summary>
/// Data that each card action will need
/// </summary>
[System.Serializable]
public class CardActionData
{
    /// <summary>This is the index of BoardElement in BoardElementClassNames, Check the scriptable object in _ScriptableObjects</summary>
    public CardSelectionType SelectionType;
    public CardActionEnum actionEnum;
    public EventReference action_SFX;
    public ActionVFX _VFX;
    public bool IsValidSelection(BoardElement target)
    {
        string targetClassName = target.GetType().Name;

        Debug.Log(targetClassName);

        if(targetClassName.Equals("None"))
            return false;

        if(targetClassName.Equals("BoardElement"))
            return true;

        int monsterIndex = BoardElementClassNames.instance.getIndexOf("Monster"); 
        int targetIndex = BoardElementClassNames.instance.getIndexOf(targetClassName);
        

        if(monsterIndex == SelectionType.Index)
        {
            if(targetClassName.Contains("Monster"))
                return true;
        }

        if(targetIndex == SelectionType.Index)
            return true;
        return false;
    }
}

[System.Serializable]
public class ActionGroup
{
    public ActionGroup()
    {
        actionStats = new List<CardActionData>();
        actions = new List<CardAction>();
    }
    public List<CardActionData> actionStats;
    public List<CardAction> actions;

    internal void Add(CardAction act)
    {
        actions.Add(act);
    }

    public ActionGroup Clone()
    {
        ActionGroup clone = new ActionGroup();
        clone.actionStats = this.actionStats;
        return clone;
    }
}

[System.Serializable]
public class CardStats
{
    public CameraView TargetingView;
    public int strength;
    public int numberOfTargets;
    public ActionGroup actionGroup;
    public GodActionEnum correspondingGod;
    public ActionGroup godBuffActions;

    public CardStats Clone()
    {
        CardStats clone = new CardStats();
        clone.strength = this.strength;
        clone.numberOfTargets = this.numberOfTargets;
        clone.actionGroup = this.actionGroup.Clone();
        clone.godBuffActions = this.godBuffActions.Clone();
        return clone;
    }
}

