// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft,
//  Nicolay Joahsen

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;
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

    public string getEffectFormatted(CardStats overrideStats = null)
    {
        string output = "Not Assigned";
        CardStats targetStats = cardStats;


        if(overrideStats != null)
            targetStats = overrideStats;

        if(effect == "")
            return output;

        if(targetStats == null)
            return effect;

        output = string.Format(effect, targetStats.strength);

        string targetName = targetStats.SelectionType.getName();
    
        if(targetStats.SelectionType.getName().Equals("ActionCard_Behaviour"))
            targetName = "Card";
        if(targetStats.SelectionType.getName().Equals("GodCard_Behaviour"))
            targetName = "God Card";
        if(targetStats.SelectionType.getName().Equals("Card_Behaviour"))
            targetName = "Any type of card";
        if(targetStats.SelectionType.getName().Equals("OfferingDebuff"))
            targetName = "Offering";

        string[] split = output.Split('|');
        
        if(split.Length > 0)
        {
            output = split[0];
        }

        for (int i = 1; i < split.Length; i++)
        {
            if(targetStats.numberOfTargets > 1)
            {   
                if(targetStats.actionGroup.actionStats[0].actionEnum.Equals(CardActionEnum.Defence))
                    output += string.Format("from up to {0} {1}s", targetStats.numberOfTargets, targetName);
                else
                    output += string.Format("to up to {0} {1}s", targetStats.numberOfTargets, targetName);
            }
            else
            {
                if(targetStats.actionGroup.actionStats[0].actionEnum.Equals(CardActionEnum.Defence))
                    output += "from one " + targetName;
                else
                    output += "to one " + targetName;
            }
        }


        bool space = true;

        for (int i = 0; i < targetStats.actionGroup.actionStats.Count; i++)
        {
            if(targetStats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Attack)
                continue;

            if(targetStats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Defence)
                continue;

            if(targetStats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Buff)
                continue;

            if(space)
            {
                output += "\n\n";   
                space = false;
            }
            output += targetStats.actionGroup.actionStats[i].actionEnum.ToString();

            if(i < targetStats.actionGroup.actionStats.Count-1)
                output += "\n";
        }

        if(split.Length > 1)
            output += split[1];
        return output;
    }
    public static string getEffectFormatted(Card_Behaviour targetCard)
    {
        string output = "Not Assigned";

        if(targetCard is ActionCard_Behaviour actionCard)
        {
            CardStats stats = actionCard.stats;
            ActionCard_ScriptableObject card_so = actionCard.CardSO;

            output = string.Format(card_so.effect, stats.strength);

            string targetName = stats.SelectionType.getName();
        
            if(stats.SelectionType.getName().Equals("ActionCard_Behaviour"))
                targetName = "Card";
            if(stats.SelectionType.getName().Equals("GodCard_Behaviour"))
                targetName = "God Card";
            if(stats.SelectionType.getName().Equals("Card_Behaviour"))
                targetName = "Any type of card";
            if(stats.SelectionType.getName().Equals("OfferingDebuff"))
                targetName = "Offering";

            string[] split = output.Split('|');
        
            if(split.Length > 0)
            {
                output = split[0];
            }

            for (int i = 1; i < split.Length; i++)
            {
                if(stats.numberOfTargets > 1)
                {   
                    if(stats.actionGroup.actionStats[0].actionEnum.Equals(CardActionEnum.Defence))
                        output += string.Format("from up to {0} {1}s", stats.numberOfTargets, targetName);
                    else
                        output += string.Format("to up to {0} {1}s", stats.numberOfTargets, targetName);
                }
                else
                {
                    if(stats.actionGroup.actionStats[0].actionEnum.Equals(CardActionEnum.Defence))
                        output += "from one " + targetName;
                    else
                        output += "to one " + targetName;
                }
                output += split[i];
            }

            bool space = true;

            for (int i = 0; i < stats.actionGroup.actionStats.Count; i++)
            {
                if(stats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Attack)
                    continue;

                if(stats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Defence)
                    continue;

                if(stats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Buff)
                    continue;

                if(space)
                {
                    // output += "\n\n";   
                    space = false;
                }

                output += stats.actionGroup.actionStats[i].actionEnum.ToString() + "\n";
            }
        }
        else if(targetCard is GodCard_Behaviour godCard)
        {
            output = godCard.CardSO.effect;
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

    public static string getEffectFormatted(Card_SO targetCard)
    {
        string output = "Not Assigned";

        if (targetCard is ActionCard_ScriptableObject actionCard)
        {
            CardStats stats = actionCard.cardStats;
            ActionCard_ScriptableObject card_so = actionCard;

            output = string.Format(card_so.effect, stats.strength);

            string targetName = stats.SelectionType.getName();

            if (stats.SelectionType.getName().Equals("ActionCard_Behaviour"))
                targetName = "Card";
            if (stats.SelectionType.getName().Equals("GodCard_Behaviour"))
                targetName = "God Card";
            if (stats.SelectionType.getName().Equals("Card_Behaviour"))
                targetName = "Any type of card";
            if (stats.SelectionType.getName().Equals("OfferingDebuff"))
                targetName = "Offering";

            string[] split = output.Split('|');

            if (split.Length > 0)
            {
                output = split[0];
            }

            for (int i = 1; i < split.Length; i++)
            {
                if (stats.numberOfTargets > 1)
                {
                    if (stats.actionGroup.actionStats[0].actionEnum.Equals(CardActionEnum.Defence))
                        output += string.Format("from up to {0} {1}s", stats.numberOfTargets, targetName);
                    else
                        output += string.Format("to up to {0} {1}s", stats.numberOfTargets, targetName);
                }
                else
                {
                    if (stats.actionGroup.actionStats[0].actionEnum.Equals(CardActionEnum.Defence))
                        output += "from one " + targetName;
                    else
                        output += "to one " + targetName;
                }
                output += split[i];
            }

            bool space = true;

            for (int i = 0; i < stats.actionGroup.actionStats.Count; i++)
            {
                if (stats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Attack)
                    continue;

                if (stats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Defence)
                    continue;

                if (stats.actionGroup.actionStats[i].actionEnum == CardActionEnum.Buff)
                    continue;

                if (space)
                {
                    output += "\n\n";
                    space = false;
                }

                output += stats.actionGroup.actionStats[i].actionEnum.ToString() + "\n";
            }
        }
        else if (targetCard is GodCard_ScriptableObject godCard)
        {
            output = godCard.effect;
        }

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
    [HideInInspector] public ActionVFX _VFX;
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

    public void SetGlyphs(CardActionEnum[] Glyphs, bool KeepBaseAction = false)
    {
        if(Upgrades == null)
            return;
     
        // prevent removal of the main action on the card. ie Attack/defence
        List<CardActionEnum> newRemovable = new List<CardActionEnum>(1+Glyphs.Length);

        // add base action if it has any
        if(Upgrades.Length > 0 && KeepBaseAction)
        {
            if(Upgrades[0].RemovableGlyph[0] == CardActionEnum.Attack || Upgrades[0].RemovableGlyph[0] == CardActionEnum.Defence)
                newRemovable.Add(Upgrades[0].RemovableGlyph[0]);
        }

        // add the remaining actions
        for (int i = 0; i < Glyphs.Length; i++)
        {
            newRemovable.Add(Glyphs[i]);
        }

        // apply
        for (int i = 0; i < Upgrades.Length; i++)
        {
            for (int j = 1; j < Upgrades[i].RemovableGlyph.Length; j++)
            {
                Upgrades[i].RemovableGlyph = newRemovable.ToArray();
            }
        }
    }

    public void logXP()
    {
        string s = "Current Xp: " + Experience.XP + "\n Current Level: " + Experience.Level;
        Debug.Log(s);
    }
}

[System.Serializable]
public class CardStats
{
    public CameraView TargetingView;
    public CardSelectionType SelectionType;
    public int strength;
    public string formattedStrength => numberOfTargets > 1 ? strength + "x" + numberOfTargets : strength.ToString();
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
        clone.correspondingGod = this.correspondingGod;
        clone.TargetingView = this.TargetingView;
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