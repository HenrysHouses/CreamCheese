using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif


[System.Serializable]
public struct CardUpgrade
{
    public int RequiredXP;
    // # Toolbar
    public CardUpgradeType Type;
    public int UpgradeTypeIndex;
    // # Remove
    /// <summary>Current Glyphs of a card, Use to select removal of glyphs</summary>
    public CardActionEnum[] RemovableGlyph;
    // # Add
    public CardActionEnum AddGlyph;
    public int RemoveGlyphIndex;
    // # Modify
    public ModifiableCardValue ValueSelection;
    public GodActionEnum CorrespondingGod;
    public CardSelectionType SelectionType;
    public int EditedValue;
}

public enum CardUpgradeType
{
    AddGlyph = 0,
    RemoveGlyph = 1,
    ModifyValue = 2
}

public enum ModifiableCardValue
{
    Strength = 0,
    NumberOfTargets = 1,
    SelectionType = 2,
    CorrespondingGod = 3
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(CardUpgrade))]
public class CardUpgrade_PropertyDrawer: PropertyDrawer 
{
    float lineCount;
    float XPOffset = 40;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        lineCount = 0;

        Rect XpRect = new Rect(position);
        position.xMin += XPOffset;

        SerializedProperty Index = property.FindPropertyRelative("UpgradeTypeIndex");
        DrawUpgradeTypeToolbar(position, property, label, Index);
        
        switch((CardUpgradeType)Index.intValue)
        {
            case CardUpgradeType.AddGlyph:
                DrawGlyphAdding(position, property, label);
                break;

            case CardUpgradeType.RemoveGlyph:
                DrawGlyphRemoval(position, property, label);
                break;

            case CardUpgradeType.ModifyValue:
                DrawModifyValue(position, property, label);
                break;
        }

        XpRect.xMax = 115;
        Rect LevelRect = new Rect(XpRect);
        LevelRect.y -= 30;
        Rect XpTextRect = new Rect(XpRect);
        XpTextRect.y -= 11;

        char[] lvlIndex = property.propertyPath.ToCharArray();
        int n = int.Parse(lvlIndex[lvlIndex.Length-2].ToString());
        GUI.Label(LevelRect, "Lvl: " + (n+1));

        GUI.Label(XpTextRect, "XP");

        SerializedProperty XP = property.FindPropertyRelative("RequiredXP");
        DrawLevelIndex(XpRect, XP, null);
    }


    // #### Draw Toolbar ####

    void DrawLevelIndex(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * 2;
        rect.y += EditorGUIUtility.standardVerticalSpacing * 2;
        rect.height = EditorGUIUtility.singleLineHeight;

        if(label != null)
            property.intValue = EditorGUI.IntField(rect, label.text, property.intValue);
        if(label == null)
            property.intValue = EditorGUI.IntField(rect, property.intValue);
        lineCount++;
    }

    void DrawUpgradeTypeToolbar(Rect position, SerializedProperty property, GUIContent label, SerializedProperty pageIndex)
    {
        SerializedProperty Types = property.FindPropertyRelative("Type");

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        pageIndex.intValue = GUI.Toolbar(rect, pageIndex.intValue, Types.enumDisplayNames);
        lineCount++;
    }

    void DrawGlyphAdding(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty Glyph = property.FindPropertyRelative("AddGlyph");

        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * lineCount;
        rect.y += EditorGUIUtility.standardVerticalSpacing;
        rect.height = EditorGUIUtility.singleLineHeight;

        Glyph.intValue = EditorGUI.Popup(rect, "Glyph", Glyph.intValue, Glyph.enumDisplayNames);
        // lineCount++;
    }

    void DrawGlyphRemoval(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty Glyph = property.FindPropertyRelative("RemovableGlyph");
        SerializedProperty Index = property.FindPropertyRelative("RemoveGlyphIndex");

        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * lineCount;
        rect.y += EditorGUIUtility.standardVerticalSpacing;
        rect.height = EditorGUIUtility.singleLineHeight;

        int[] indices = getEnumsInArray(Glyph);
        string[] _Enum = new string[indices.Length];
        
        for (int i = 0; i < _Enum.Length; i++)
        {
            CardActionEnum n = (CardActionEnum)indices[i];
            _Enum[i] = n.ToString();
        }

        Index.intValue = EditorGUI.Popup(rect, "Glyph", Index.intValue, _Enum);
        lineCount++;
    }

    void DrawModifyValue(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty Modify = property.FindPropertyRelative("ValueSelection");

        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * lineCount;
        rect.y += EditorGUIUtility.standardVerticalSpacing;
        rect.height = EditorGUIUtility.singleLineHeight;

        Modify.intValue = EditorGUI.Popup(rect, "Select Value", Modify.intValue, Modify.enumDisplayNames);
        lineCount++;

        switch((ModifiableCardValue)Modify.intValue)
        {
            case ModifiableCardValue.Strength:
            case ModifiableCardValue.NumberOfTargets:
                SerializedProperty intVariable = property.FindPropertyRelative("EditedValue");
                DrawIntValue(position, null, intVariable);
                break;
            
            case ModifiableCardValue.SelectionType:
                SerializedProperty _Selection = property.FindPropertyRelative("SelectionType");
                DrawSelectableTargets(position, label, _Selection);
                break;
            
            case ModifiableCardValue.CorrespondingGod:
                SerializedProperty _God = property.FindPropertyRelative("CorrespondingGod");
                DrawEnum(position, label, _God);
                break;
        }
    }

    // #### Draw Modifiable value GUI ####

    void DrawIntValue(Rect position, GUIContent label, SerializedProperty Variable)
    {
        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * lineCount;
        rect.y += EditorGUIUtility.standardVerticalSpacing * 2;
        rect.height = EditorGUIUtility.singleLineHeight;

        if(label != null)
            Variable.intValue = EditorGUI.IntField(rect, label.text, Variable.intValue);
        if(label == null)
            Variable.intValue = EditorGUI.IntField(rect, Variable.intValue);
        lineCount++;
    }

    void DrawEnum(Rect position, GUIContent label, SerializedProperty Variable)
    {
        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * lineCount;
        rect.y += EditorGUIUtility.standardVerticalSpacing * 2;
        rect.height = EditorGUIUtility.singleLineHeight;

        Variable.intValue = EditorGUI.Popup(rect, Variable.intValue, Variable.enumDisplayNames);
        lineCount++;
    }

    void DrawSelectableTargets(Rect position, GUIContent label, SerializedProperty Variable)
    {
        Rect rect = position;
        rect.y += EditorGUIUtility.singleLineHeight * lineCount;
        rect.y += EditorGUIUtility.standardVerticalSpacing * 2;
        rect.height = EditorGUIUtility.singleLineHeight;

        SerializedProperty Index = Variable.FindPropertyRelative("Index");
        SerializedProperty ClassNameContainer = Variable.FindPropertyRelative("NamesContainer");
        // BoardElementClassNames elementClassNames = ClassNameContainer.objectReferenceValue as BoardElementClassNames;

        

        Index.intValue = EditorGUI.Popup(rect, Index.intValue, BoardElementClassNames.instance.Names);
        lineCount++;
    }

    // #### Utility #####

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * lineCount + EditorGUIUtility.standardVerticalSpacing * (lineCount-1);
    }

    int[] getEnumsInArray(SerializedProperty property)
    {
        int length = property.arraySize;
        int[] array = new int[length];

        for (int i = 0; i < length; i++)
        {
            SerializedProperty Element = property.GetArrayElementAtIndex(i);
            array[i] = Element.intValue;
        }
        return array;
    }
}
#endif