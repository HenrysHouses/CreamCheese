using UnityEditor;
using UnityEngine;

// Custom serializable class
[System.Serializable]
public class CardSelectionType
{
    public int Index = 0;
    public BoardElementClassNames Names;
}

#if UNITY_EDITOR

// drawer
[CustomPropertyDrawer(typeof(CardSelectionType))]
public class IngredientDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty index = property.FindPropertyRelative("Index");
        SerializedProperty ScriptObj = property.FindPropertyRelative("Names"); 
        
        ScriptObj.objectReferenceValue = Resources.Load<BoardElementClassNames>("BoardElementClassNamesContainer");
        BoardElementClassNames elementClassNames = ScriptObj.objectReferenceValue as BoardElementClassNames;
        // Calculate rects
        var rect = new Rect(position.x, position.y, position.width, position.height);

        index.intValue = EditorGUI.Popup(rect, "Selection Type", index.intValue, elementClassNames.Names);
    }
}
#endif