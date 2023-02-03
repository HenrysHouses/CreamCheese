using UnityEditor;
using UnityEngine;

// Custom serializable class
[System.Serializable]
public class CardSelectionType
{
    public int Index = 0;
}

// drawer
[CustomPropertyDrawer(typeof(CardSelectionType))]
public class IngredientDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty index = property.FindPropertyRelative("Index");
        
        // Calculate rects
        var rect = new Rect(position.x, position.y, position.width, position.height);
        index.intValue = EditorGUI.Popup(rect, "Selection Type", index.intValue, BoardElementClassNames.instance.Names);
    }
}