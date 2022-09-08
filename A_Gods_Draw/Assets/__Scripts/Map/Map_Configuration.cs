//CHARLIE SCrIPT
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;
using Malee;

namespace Map
{
    [CreateAssetMenu]
    public class Map_Configuration : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;

        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        public MinMaxInt numOfPreBossNodes;
        public MinMaxInt numOfStartingNodes;

        [Reorderable]
        public ListOfMapLayers layers;

        [System.Serializable]
        public class ListOfMapLayers : ReorderableArray<Map_Layer>
        {

        }
    }
}

