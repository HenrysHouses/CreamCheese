//CHARLIE SCrIPT
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using System;

namespace Map
{
    [CreateAssetMenu]
    public class Map_Configuration : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;

        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        public MinMaxInt numOfPreBossNodes;
        public MinMaxInt numOfStartingNodes;
        public ListOfMapLayers layers;

        [System.Serializable]
        public class ListOfMapLayers : List<Map_Layer>
        {
        }
    }
}

