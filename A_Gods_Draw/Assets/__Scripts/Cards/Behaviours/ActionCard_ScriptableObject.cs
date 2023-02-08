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

    private void OnValidate() {
        cardStats.UpgradePath.SetGlyphs(cardStats.getGlyphs(CardType.None));
    }


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
    public CardExperience Experience;
    public CardUpgrade[] Upgrades;

    public void SetGlyphs(CardActionEnum[] Glyphs)
    {
        for (int i = 0; i < Upgrades.Length; i++)
        {
            Upgrades[i].RemovableGlyph = Glyphs;
        }
    }
}

public struct CardExperience
{
    public int XP;
    public int Level;
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

    public CardActionEnum[] getGlyphs(CardType type)
    {
        List<CardActionEnum> allGlyphs = new List<CardActionEnum>();
        foreach (var _action in actionGroup.actionStats)
        {
            if(_action.actionEnum.ToString() == type.ToString())
                continue;

            allGlyphs.Add(_action.actionEnum);
        }

        return allGlyphs.ToArray();
    }
}