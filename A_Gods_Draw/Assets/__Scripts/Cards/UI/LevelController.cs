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
    GodCard_ScriptableObject _God;
    CardUpgradePath upgradePath;
    CardExperience experience;
    [SerializeField] Texture2D[] LevelTextures;

    List<GameObject> SpawnedGlyphs = new List<GameObject>();
    GameObject SpawnedGodGlyph;
    List<CardActionEnum> glyphsOrder = new List<CardActionEnum>();
    bool HasGodGlyph;
    bool SkipSetWhenDestroyed;
    
    public void set(Card_Selector selector, CardPlayData data, bool spawnAsDisplay = false)
    {
        Card_Selector = selector;
        experience = data.Experience;

        if(data.CardType is not ActionCard_ScriptableObject ActionCard)
        {
            _God = data.CardType as GodCard_ScriptableObject;
            GodActionEnum GodAction = _God.godAction;

            instantiateGodIcon(GodAction, spawnAsDisplay);

            if(SkipSetWhenDestroyed)
            {
                gameObject.SetActive(false);
                Destroy(this);
            }
        }
        else
        {
            _Card = ActionCard;
            upgradePath = _Card.cardStats.UpgradePath;
        }
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

        tex = LevelTextures[targetLevel];

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

    /// <summary>Spawn the designated god glyph</summary>
    /// <param name="spawnAsDisplay">Dont know why this was required but it was needed outside of combat</param>
    public void instantiateGodIcon(GodActionEnum God, bool spawnAsDisplay = false)
    {
        if(God == GodActionEnum.None)
            return;

        if(SpawnedGodGlyph != null)
            Destroy(SpawnedGodGlyph);

        GameObject icon = Instantiate(IconPrefab);

        if(spawnAsDisplay)
            IconPath.recalculatePath();
        OrientedPoint OP = IconPath.GetEvenPathOP(0.5f);
        icon.transform.position = OP.pos;
        icon.transform.SetParent(IconPath.transform.parent, spawnAsDisplay);
        icon.transform.localEulerAngles = Vector3.zero;
        icon.transform.localScale = Vector3.one;
        icon.GetComponent<GlyphController>().setGlyph(_God.godAction, Card_Selector, "", true);

        SpawnedGodGlyph = icon;
    }

    /// <summary>Spawn Glyph Icons on Action Cards</summary>
    /// <param name="glyphs">Which glyphs to spawn on the card</param>
    /// <param name="spawnAsDisplay">Dont know why this was required but it was needed outside of combat</param>
    public void instantiateIcons(CardActionEnum[] glyphs, bool spawnAsDisplay = false, bool reset = false)
    {
        if(reset)
        {
            for (int i = 0; i < SpawnedGlyphs.Count; i++)
            {
                Destroy(SpawnedGlyphs[i]);
            }

            HasGodGlyph = false;
            SpawnedGlyphs.Clear();
        }

        float godGlyph = 0;
        if(_Card.cardStats.correspondingGod != GodActionEnum.None && !HasGodGlyph)
            godGlyph = 1;

        if(glyphs == null && godGlyph == 0)
            return;

        if(glyphs.Length <= 0 && godGlyph == 0)
            return;


        float pos = 1/(glyphs.Length+1f+godGlyph);

        for (int i = 0; i < glyphs.Length + godGlyph; i++)
        {
            GameObject icon = Instantiate(IconPrefab);
            OrientedPoint OP = IconPath.GetEvenPathOP(pos * (i+1));
            icon.transform.position = OP.pos;
            icon.transform.SetParent(IconPath.transform.parent, spawnAsDisplay);
            icon.transform.localEulerAngles = Vector3.zero;
            icon.transform.localScale = Vector3.one;

            if(godGlyph == 1 && i == 0)
            {
                icon.GetComponent<GlyphController>().setGlyph(_Card.cardStats.correspondingGod, Card_Selector, _Card.getGodBuffActions());
                // glyphsOrder.Add(_Card.cardStats.correspondingGod);
                HasGodGlyph = true;
            }
            else if(godGlyph == 1)
            {
                icon.GetComponent<GlyphController>().setGlyph(glyphs[i-1], Card_Selector);
                glyphsOrder.Add(glyphs[i-1]);
            }
            else
            {
                icon.GetComponent<GlyphController>().setGlyph(glyphs[i], Card_Selector);
                glyphsOrder.Add(glyphs[i]);
            }

            SpawnedGlyphs.Add(icon);
        }
    }

    public void destroyGlyph(CardActionEnum Glyph)
    {
        for (int i = 0; i < glyphsOrder.Count; i++)
        {
            if(Glyph == glyphsOrder[i])
            {
                if(SpawnedGlyphs[i].GetComponent<GlyphController>().isGodGlyph)
                    continue;

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

    void OnDestroy()
    {
        SkipSetWhenDestroyed = true;
    }
}
