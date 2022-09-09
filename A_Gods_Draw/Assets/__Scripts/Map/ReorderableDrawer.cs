//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Malee.Editor
{
    [CustomPropertyDrawer(typeof(ReorderableAttribute))]
    public class ReorderableDrawer : PropertyDrawer
    {
        public const string ARRAY_PROPERTY_NAME = "array";
        private static Dictionary<int, ReorderableList> lists = new Dictionary<int, ReorderableList>();
        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ReorderableList list = GetList(property, attribute as ReorderableAttribute, ARRAY_PROPERTY_NAME);
            return list != null ? list.GetHeight() : EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReorderableList list = GetList(property, attribute as ReorderableAttribute, ARRAY_PROPERTY_NAME);

            if(list != null)
            {
                list.DoList(EditorGUI.IndentedRect(position), label); //FIX
            }
            else
            {
                GUI.Label(position, "The Array MUST extend from ReorderableArray", EditorStyles.label);
            }
        }

        public static int GetListId(SerializedProperty property)
        {
            if(property != null)
            {
                int h1 = property.serializedObject.targetObject.GetHashCode();
                int h2 = property.propertyPath.GetHashCode();

                return (((h1 << 5) + h1) ^ h2);
            }

            return 0;
        }

        public static ReorderableList GetList(SerializedProperty property, string arrayPropertyName)
        {
            return GetList(property, null, GetListId(property), arrayPropertyName);
        }

        public static ReorderableList GetList(SerializedProperty property, ReorderableAttribute attribute, string arrayPropertyName)
        {
            return GetList(property, attribute, GetListId(property), arrayPropertyName);
        }

        public static ReorderableList GetList(SerializedProperty property, int id, string arrayPropertyName)
        {
            return GetList(property, null, id, arrayPropertyName);
        }

        public static ReorderableList GetList(SerializedProperty property, ReorderableAttribute attribute, int id, string arrayPropertyName)
        {
            if(property == null)
            {
                return null;
            }

            ReorderableList list = null;
            SerializedProperty array = property.FindPropertyRelative(arrayPropertyName);

            if(array != null && array.isArray)
            {
                if(!lists.TryGetValue(id, out list))
                {
                    if(attribute != null)
                    {
                        Texture icon = !string.IsNullOrEmpty(attribute.elementIconPath) ? AssetDatabase.GetCachedIcon(attribute.elementIconPath) : null;
                        ReorderableList.ElementDisplayType displayType = attribute.singleLine ? ReorderableList.ElementDisplayType.SingleLine : ReorderableList.ElementDisplayType.Auto;

                        list = new ReorderableList(array, attribute.add, attribute.remove, attribute.draggable, displayType, attribute.elementNameProperty, attribute.elementNameOverride, icon);
                        list.seperate = attribute.seperate;
                        list.pageSize = attribute.pageSize;
                        list.sortable = attribute.sortable;

                        if(attribute.surrogateType != null)
                        {
                            SurrogateCallback callback = new SurrogateCallback(attribute.surrogateProperty);
                            list.surrogate = new ReorderableList.Surrogate(attribute.surrogateType, callback.SetReference);
                        }
                    }
                    else
                    {
                        list = new ReorderableList(array, true, true, true);
                    }

                    lists.Add(id, list);
                }
                else
                {
                    list.List = array;
                }
            }

            return list;
        }

        private struct SurrogateCallback
        {
            private string property;
            internal SurrogateCallback(string property)
            {
                this.property = property;
            }

            internal void SetReference(SerializedProperty element, Object objRef, ReorderableList list)
            {
                SerializedProperty prop = !string.IsNullOrEmpty(property) ? element.FindPropertyRelative(property) : null;

                if(prop != null && prop.propertyType == SerializedPropertyType.ObjectReference)
                {
                    prop.objectReferenceValue = objRef;
                }
            }
        }
    }
}

