//charlie
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Map
{

    [CustomEditor(typeof(Map_Manager))]
    
    public class Map_ManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Map_Manager myScript = (Map_Manager)target;
            GUILayout.Space(10);

            if (GUILayout.Button("Generate"))
            {
                myScript.GenerateNewMap();
            }
        }
    }

}
#endif
