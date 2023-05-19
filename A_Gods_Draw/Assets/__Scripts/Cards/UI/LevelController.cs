using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    [SerializeField] PathController IconPath;
    [SerializeField] GameObject IconPrefab;
    public Material materialInstance => _renderer.material;
    [SerializeField] UIPopup Description;
    Card_Selector Card_Selector;
    ActionCard_ScriptableObject _Card;
    CardUpgradePath upgradePath;
    CardExperience experience;

    List<GameObject> SpawnedGlyphs = new List<GameObject>();
    List<CardActionEnum> glyphsOrder = new List<CardActionEnum>();
    
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

        if(level < upgradePath.Upgrades.Length && level > -1)
        {
            // Debug.Log(level + " < " + upgradePath.Upgrades.Length);

            unlocks += "\nNext Unlock:\n";
            unlocks += upgradePath.Upgrades[level].getDescription(true);
        }

        Description.setDescription(unlocks);
    }

    public string setDescriptionShowAllLevels()
    {
        if(upgradePath.Upgrades == null)
            return "-1";

        string unlocks = "";
        for (int i = 0; i < upgradePath.Upgrades.Length; i++)
        {
            if(i >= upgradePath.Upgrades.Length)
                break;

            if(i == 0)
                unlocks += "";

            unlocks += "Level " + i + ": " + upgradePath.Upgrades[i].getDescription(true) + "\n";
        }

        Description.setDescription(unlocks);
        return unlocks;
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

        if(targetLevel > 0 && targetLevel < Upgrades.Length)
        {
            previousRequirement = Upgrades[targetLevel-1].RequiredXP;
            NeededForLevel = Upgrades[targetLevel].RequiredXP - previousRequirement;
        }

        float progress = (Experience.XP - previousRequirement) / NeededForLevel;

        materialInstance.SetTexture("_LevelTexture", tex);

        if(MaxLevel)
            materialInstance.SetFloat("_CircleFill", 1);
        else
            materialInstance.SetFloat("_CircleFill", progress);
        return true;
    }

    public void instantiateIcons(CardActionEnum[] glyphs, bool spawnAsDisplay = false)
    {
        if(glyphs == null)
            return;

        if(glyphs.Length <= 0)
            return;

        float pos = 1/(glyphs.Length+1f);

        for (int i = 0; i < glyphs.Length; i++)
        {
            GameObject icon = Instantiate(IconPrefab);
            OrientedPoint OP = IconPath.GetEvenPathOP(pos * (i+1));
            icon.transform.position = OP.pos;
            icon.transform.SetParent(IconPath.transform.parent, spawnAsDisplay);
            icon.transform.localEulerAngles = Vector3.zero;
            icon.transform.localScale = Vector3.one;

            icon.GetComponent<GlyphController>().setGlyph(glyphs[i], Card_Selector);
            SpawnedGlyphs.Add(icon);
            glyphsOrder.Add(glyphs[i]);
        }
    }

    public void destroyGlyph(CardActionEnum Glyph)
    {
        for (int i = 0; i < glyphsOrder.Count; i++)
        {
            if(Glyph == glyphsOrder[i])
            {
                glyphsOrder.RemoveAt(i);
                Destroy(SpawnedGlyphs[i]);
                SpawnedGlyphs.RemoveAt(i);
            }
        }
        updateGlyphPositions();
    }

    public void updateGlyphPositions()
    {
        IconPath.recalculatePath();

        if(SpawnedGlyphs.Count <= 0)
            return;

        float pos = 1/(SpawnedGlyphs.Count+1f);

        for (int i = 0; i < SpawnedGlyphs.Count; i++)
        {
            OrientedPoint OP = IconPath.GetEvenPathOP(pos * (i+1));
            SpawnedGlyphs[i].transform.position = OP.pos;
            SpawnedGlyphs[i].transform.localEulerAngles = Vector3.zero;
            SpawnedGlyphs[i].transform.localScale = Vector3.one;
        }
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
