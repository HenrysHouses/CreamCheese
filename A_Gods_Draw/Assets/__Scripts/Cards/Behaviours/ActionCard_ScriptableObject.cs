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
}

/// <summary>
/// Data that each card action will need
/// </summary>
[System.Serializable]
public class CardActionData
{
    /// <summary>This is the index of BoardElement in BoardElementClassNames, Check the scriptable object in _ScriptableObjects</summary>
    // public CardSelectionType SelectionOverride;
    public CardActionEnum actionEnum;
    public EventReference action_SFX;
    public ActionVFX _VFX;
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
public struct CardUpgradePath
{
    public CardUpgrade[] Upgrades;

    public void SetGlyphs(CardActionEnum[] Glyphs)
    {
        for (int i = 0; i < Upgrades.Length; i++)
        {
            Upgrades[i].CurrentGlyphs = Glyphs;
        }
    }
}

[System.Serializable]
public class CardStats
{
    public CameraView TargetingView;
    public CardSelectionType SelectionType;
    public int strength;
    public int numberOfTargets;
    public ActionGroup actionGroup;
    public GodActionEnum correspondingGod;
    public ActionGroup godBuffActions;
    public CardUpgradePath UpgradePath;

    public CardStats Clone()
    {
        CardStats clone = new CardStats();
        clone.strength = this.strength;
        clone.numberOfTargets = this.numberOfTargets;
        clone.actionGroup = this.actionGroup.Clone();
        clone.godBuffActions = this.godBuffActions.Clone();
        clone.SelectionType = new CardSelectionType();
        clone.SelectionType.Index = this.SelectionType.Index;
        return clone;
    }

    private void OnValidate() {
        UpgradePath.SetGlyphs(getGlyphs(CardType.None));
    }

    public CardActionEnum[] getGlyphs(CardType type)
    {
        List<CardActionEnum> allGlyphs = new List<CardActionEnum>();
        for (int i = 0; i < actionGroup.actions.Count; i++)
        {
            foreach (var _action in actionGroup.actionStats)
            {
                if(_action.actionEnum.ToString() == type.ToString())
                    continue;

                allGlyphs.Add(_action.actionEnum);
            }
        }
        return allGlyphs.ToArray();
    }
}