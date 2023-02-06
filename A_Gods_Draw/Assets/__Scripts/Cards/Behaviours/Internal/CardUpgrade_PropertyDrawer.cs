using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif


[System.Serializable]
public struct CardUpgrade
{
    [HideInInspector] public CardActionEnum[] CurrentGlyphs;
    [HideInInspector] public CardUpgradeType Type;
    [HideInInspector] public int UpgradeTypeIndex;
    public int myInt;
}

public enum CardUpgradeType
{
    Add = 0,
    Remove = 1,
    ModifyValue = 2
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(CardUpgrade))]
public class CardUpgrade_PropertyDrawer: PropertyDrawer 
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        base.OnGUI(position, property, label);

        SerializedProperty Index = property.FindPropertyRelative("UpgradeTypeIndex");
        UpgradeTypeToolbar(position, property, label, Index);
        
        switch(Index.intValue)
        {
            case 0:
            case 1:
                break;

            case 2:
                break;
        }
    }

    void UpgradeTypeToolbar(Rect position, SerializedProperty property, GUIContent label, SerializedProperty pageIndex)
    {
        SerializedProperty Types = property.FindPropertyRelative("Type");

        pageIndex.intValue = GUI.Toolbar(position, pageIndex.intValue, Types.enumDisplayNames);
    }
}
#endif