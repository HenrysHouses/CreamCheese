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

    public string getEffectFormatted()
    {
        string output;
        try
        {
            output = string.Format(effect, cardStats.strength);
        }
        catch
        {
            output = effect;
        }

        if(cardStats.numberOfTargets > 1)
        {
            output += string.Format(" to {0} targets", cardStats.numberOfTargets);
        }
        else
        {
            output += " to one target";
        }

        bool space = true;

        for (int i = 0; i < cardStats.actionGroup.actionStats.Count; i++)
        {
            if(cardStats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Attack)
                continue;

            if(cardStats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Defence)
                continue;

            if(cardStats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Buff)
                continue;

            if(space)
            {
                output += "\n\n";   
                space = false;
            }

            output += cardStats.actionGroup.actionStats[i].actionEnum.ToString() + "\n";
        }
        return output;
    }

    public string getFormattedStrength()
    {
        string output = cardStats.strength.ToString();

        if(cardStats.numberOfTargets > 1)
            output += "x" + cardStats.numberOfTargets;

        return output;
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
        clone.actionStats = new List<CardActionData>();

        // clone.actionStats[i]
        CardActionData[] _copy = new CardActionData[this.actionStats.Count];
        this.actionStats.CopyTo(_copy);
        
        for (int j = 0; j < _copy.Length; j++)
        {
            clone.actionStats.Add(_copy[j]);
        }

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
        if(Upgrades == null)
            return;
     
        for (int i = 0; i < Upgrades.Length; i++)
        {
            Upgrades[i].RemovableGlyph = Glyphs;
        }
    }

    public void logXP()
    {
        string s = "Current Xp: " + Experience.XP + "\n Current Level: " + Experience.Level;
        Debug.Log(s);
    }
}

[System.Serializable]
public struct CardExperience
{
    public int XP;
    public int Level;
    public int ID;
    static List<int> ExistingIDs = new List<int>();

    public CardExperience(int _XP, int _Lvl, int _ID)
    {
        XP = _XP;
        Level = _Lvl;
        
        if(_ID < 0)
        {
            ID = 0;
            this.createNewUniqueID();
        }
        else
            ID = _ID;
    }

    public void createNewUniqueID()
    {
        if(ExistingIDs.Contains(ID))
            return;

        ID = 0;
        while(ExistingIDs.Contains(ID))
        {
            ID++;
        }
        ExistingIDs.Add(ID);
    }

    public static void ClearUnusedIDs(DeckListData deck)
    {
        List<int> _foundIDs = new List<int>();
        for (int i = 0; i < deck.Count; i++)
        {
            _foundIDs.Add(deck.deckListData[i].Experience.ID);
        }

        List<int> _removeIds = new List<int>();
        for (int i = 0; i < ExistingIDs.Count; i++)
        {
            if(_foundIDs.Contains(ExistingIDs[i]))
                continue;

            _removeIds.Add(ExistingIDs[i]);
        }

        for (int i = 0; i < _removeIds.Count; i++)
        {
            ExistingIDs.Remove(_removeIds[i]);
        }
    }
}

[System.Serializable]
public class CardStats
{
    public CameraView TargetingView;
    public CardSelectionType SelectionType;
    public int strength;
    public int numberOfTargets = 1;
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
        clone.UpgradePath = this.UpgradePath;
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