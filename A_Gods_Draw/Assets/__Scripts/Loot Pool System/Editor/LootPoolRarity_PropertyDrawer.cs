#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LootPoolRarity))]
public class LootPoolRarity_PropertyDrawer: PropertyDrawer 
{
    float linesDrawn;
    float space;

    const string Common = "Common";
    const string Uncommon = "Uncommon";
    const string Rare = "Rare";
    const string Legendary = "Legendary";
    const string Unique = "Unique";

    // Declare styles
    static GUIStyle CommonStyle = new GUIStyle(EditorStyles.boldLabel);
    static GUIStyle UncommonStyle = new GUIStyle(EditorStyles.boldLabel);
    static GUIStyle RareStyle = new GUIStyle(EditorStyles.boldLabel);
    static GUIStyle LegendaryStyle = new GUIStyle(EditorStyles.boldLabel);
    static GUIStyle UniqueStyle = new GUIStyle(EditorStyles.boldLabel);
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        linesDrawn = 0;
        space = 0;

        // Draw rarities
        GUIRarityChance(position, property, new GUIContent(Common), CommonStyle, out float Chance_Common, out Object LootPool_Common);
        space += 10;
        GUIRarityChance(position, property, new GUIContent(Uncommon), UncommonStyle, out float Chance_Uncommon, out Object LootPool_Uncommon);
        space += 10;
        GUIRarityChance(position, property, new GUIContent(Rare), RareStyle, out float Chance_Rare, out Object LootPool_Rare);
        space += 10;
        GUIRarityChance(position, property, new GUIContent(Legendary), LegendaryStyle, out float Chance_Legendary, out Object LootPool_Legendary);
        space += 10;
        GUIRarityChance(position, property, new GUIContent(Unique), UniqueStyle, out float Chance_Unique, out Object LootPool_Unique);
    
        float totalChance = Chance_Common + Chance_Uncommon + Chance_Rare + Chance_Legendary + Chance_Unique;

        // Warn about chances
        if(totalChance != 100)
        {
            space += 10;
            CommonStyle.normal.textColor = Color.yellow;
            GUIStyle style = new GUIStyle(CommonStyle);
            UncommonStyle.normal.textColor = Color.yellow;
            RareStyle.normal.textColor = Color.yellow;
            LegendaryStyle.normal.textColor = Color.yellow;
            UniqueStyle.normal.textColor = Color.yellow;
            EditorGUI.LabelField(newLineRect(position) ," WARNING: Total chance do not equal 100%. Rarities sum up to " + totalChance + "/100" , style);
        }
        else
        {
            CommonStyle.normal.textColor = Color.white;
            UncommonStyle.normal.textColor = Color.white;
            RareStyle.normal.textColor = Color.white;
            LegendaryStyle.normal.textColor = Color.white;
            UniqueStyle.normal.textColor = Color.white;
        }

        // Warn about null loot pools
        GUILootPoolWarning(position, Chance_Common, LootPool_Common, CommonStyle, Common);
        GUILootPoolWarning(position, Chance_Uncommon, LootPool_Uncommon, UncommonStyle, Uncommon);
        GUILootPoolWarning(position, Chance_Rare, LootPool_Rare, RareStyle, Rare);
        GUILootPoolWarning(position, Chance_Legendary, LootPool_Legendary, LegendaryStyle, Legendary);
        GUILootPoolWarning(position, Chance_Unique, LootPool_Unique, UniqueStyle, Unique);

    }

    Rect newLineRect(Rect position) 
    {
        Rect newLine = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (float)linesDrawn + space, position.width, EditorGUIUtility.singleLineHeight);
        linesDrawn++;
        return newLine;
    } 

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return  EditorGUIUtility.singleLineHeight * linesDrawn + space;
    }

    private void GUILootPoolWarning(Rect position, float chance, Object item, GUIStyle style, string Rarity)
    {
        if(chance > 0)
        {
            if(item == null)
            {
                space += 10;
                style.normal.textColor = new Color(1f, 0.479132f, 0f);
                EditorGUI.LabelField(newLineRect(position) ," WARNING: The " + Rarity +" loot pool is null!", style);
            }
        }
    }

    /// <summary>Draws the loot pool rarity</summary>
    private SerializedProperty GUIRarityChance(Rect _position, SerializedProperty _property, GUIContent _label, GUIStyle LabelStyle, out float Chance, out Object LootPool)
    {
        // Draw label
        SerializedProperty _RarityChance = _property.FindPropertyRelative(_label.text);
        Rect labelPosition = newLineRect(_position);
        EditorGUI.LabelField(labelPosition ,_label.text, LabelStyle);

        // Draw drop chance
        SerializedProperty _Chance = _RarityChance.FindPropertyRelative("Chance");
        Rect chancePosition = newLineRect(_position);
        _Chance.floatValue = EditorGUI.Slider(chancePosition, new GUIContent("Chance"), _Chance.floatValue, 0, 100);

        // Draw loot pool
        SerializedProperty _loot = _RarityChance.FindPropertyRelative("ItemPool");

        Rect lootPosition = newLineRect(_position);
        _loot.objectReferenceValue = EditorGUI.ObjectField(lootPosition,"Item Pool" ,_loot.objectReferenceValue, typeof(ItemPool_ScriptableObject), false);
    
        Chance = _Chance.floatValue;
        LootPool = _loot.objectReferenceValue;
        return _RarityChance;
    }
}
#endif