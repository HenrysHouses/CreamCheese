using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    public Material materialInstance => _renderer.material;
    [SerializeField] UIPopup Description;
    Card_Selector Card_Selector;
    ActionCard_ScriptableObject _Card;
    CardUpgradePath upgradePath;
    CardExperience experience;
    
    public void set(Card_Selector selector, CardPlayData data)
    {
        if(data.CardType is not ActionCard_ScriptableObject ActionCard)
        {
            gameObject.SetActive(false);
            return;
        }

        Card_Selector = selector;
        _Card = ActionCard;
        upgradePath = _Card.cardStats.UpgradePath;
        experience = data.Experience;
    }

    public void setDescriptionToLevel(int level)
    {
        if(upgradePath.Upgrades == null)
            return;

        int progress = CardExperience.getLevelProgress(upgradePath.Upgrades, experience);

        string unlocks = "Level progress: " + progress + "%\n\n";
        for (int i = 0; i < level; i++)
        {
            if(i >= upgradePath.Upgrades.Length)
                break;

            if(i == 0)
                unlocks += "Unlocks:\n";

            unlocks += upgradePath.Upgrades[i].getDescription(false);
        }

        if(level < upgradePath.Upgrades.Length)
        {
            unlocks += "\nNext Unlock:\n";
            unlocks += upgradePath.Upgrades[level].getDescription(true);
        }

        Description.setDescription(unlocks);
    }

    public void setDescriptionShowAllLevels()
    {
        if(upgradePath.Upgrades == null)
            return;

        string unlocks = "";
        for (int i = 0; i < upgradePath.Upgrades.Length; i++)
        {
            if(i >= upgradePath.Upgrades.Length)
                break;

            if(i == 0)
                unlocks += "Unlocks:\n";

            unlocks += "Level " + i + ": " + upgradePath.Upgrades[i].getDescription(true) + "\n";
        }

        Description.setDescription(unlocks);
    }

    public bool UpdateLevelFill(CardUpgrade[] Upgrades, CardExperience Experience, bool MaxLevel = false)
    {
        if(materialInstance == null)
            return false;

        if(Upgrades == null)
            return false;

        if(Upgrades.Length <= 0)
            return false;

        Texture tex = null;

        int targetLevel;
        if(MaxLevel)
            targetLevel = Upgrades.Length;
        else
            targetLevel = Experience.Level;

        tex = Resources.Load<Texture>("Textures/LineCount_" + targetLevel);

        float previousRequirement = 0; 
        float NeededForLevel = Upgrades[0].RequiredXP; 

        if(targetLevel > 1)
        {
            previousRequirement = Upgrades[targetLevel-2].RequiredXP;
            NeededForLevel = Upgrades[targetLevel-1].RequiredXP - previousRequirement;
        }

        float progress = (Experience.XP - previousRequirement) / NeededForLevel;

        materialInstance.SetTexture("_LevelTexture", tex);

        if(MaxLevel)
            materialInstance.SetFloat("_CircleFill", 1);
        else
            materialInstance.SetFloat("_CircleFill", progress);
        return true;
    }

    void OnMouseEnter()
    {
        if(Card_Selector)
            Card_Selector.OnMouseEnter();
    }

    void OnMouseExit()
    {
        if(Card_Selector)
            Card_Selector.OnMouseExit();
    }
}
