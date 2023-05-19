using System;
using UnityEngine;
using UnityEditor;
using HH.PropertyAttributes;

[Serializable]
public struct CardExperience
{
    public int XP;
    public int Level;
    [ReadOnly]
    public string ID;
    // static List<int> ExistingIDs = new List<int>();

    public CardExperience(int _XP, int _Lvl, string _ID)
    {
        XP = _XP;
        Level = _Lvl;

        if(_ID == "" || _ID == "Null" || _ID == null)
        {
            ID = Guid.NewGuid().ToString();    
        }
        else
        {
            ID = _ID;
        }
    }

    public void createGUID()
    {
        this.ID = Guid.NewGuid().ToString();
    }

    public static int getLevelProgress(CardUpgrade[] upgrades, CardExperience currentExperience)
    {
        if(upgrades.Length == 0)
            return 0;

        float previousRequirement = 0; 
        float NeededForLevel = upgrades[0].RequiredXP; 

        if(currentExperience.Level > 0 && currentExperience.Level < upgrades.Length)
        {
            Debug.LogWarning(currentExperience.Level + " / " + upgrades.Length);
            previousRequirement = upgrades[currentExperience.Level-1].RequiredXP;
            NeededForLevel = upgrades[currentExperience.Level].RequiredXP - previousRequirement;
        }

        float progress = (currentExperience.XP - previousRequirement) / NeededForLevel;
        progress *= 100;
        

        return (int)progress;
    }

    

    // public static void ClearUnusedIDs(DeckListData deck)
    // {
    //     List<int> _foundIDs = new List<int>();
    //     for (int i = 0; i < deck.Count; i++)
    //     {
    //         _foundIDs.Add(deck.deckListData[i].Experience.ID);
    //     }

    //     List<int> _removeIds = new List<int>();
    //     for (int i = 0; i < ExistingIDs.Count; i++)
    //     {
    //         if(_foundIDs.Contains(ExistingIDs[i]))
    //             continue;

    //         _removeIds.Add(ExistingIDs[i]);
    //     }

    //     for (int i = 0; i < _removeIds.Count; i++)
    //     {
    //         ExistingIDs.Remove(_removeIds[i]);
    //     }
    // }
}

// #if UNITY_EDITOR
// [CustomPropertyDrawer(typeof(CardExperience))]
// public class CardExperience_Property: PropertyDrawer {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
//     {
//         SerializedProperty _XP = property.FindPropertyRelative("XP");
//         SerializedProperty _level = property.FindPropertyRelative("Level");
//         SerializedProperty _DisplayID = property.FindPropertyRelative("ID");

//         position.height = EditorGUIUtility.singleLineHeight;
//         Rect XP_rect = new Rect(position);
//         Rect level_rect = new Rect(XP_rect);
//         level_rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//         Rect GUID_rect = new Rect(level_rect);
//         GUID_rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

//         _XP.intValue = EditorGUI.IntField(XP_rect, _XP.displayName ,_XP.intValue);
//         _level.intValue = EditorGUI.IntField(level_rect, _level.displayName ,_level.intValue);
//         GUI.enabled = false;
//         EditorGUI.TextField(GUID_rect, _DisplayID.displayName, _DisplayID.stringValue);
//         GUI.enabled = true;
//     }

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)*3;
//     }
// }
// #endif