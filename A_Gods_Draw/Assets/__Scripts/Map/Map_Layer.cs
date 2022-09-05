using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class Map_Layer
    {
        public NodeType nodeType;

        public MinMaxFloat distLayers;

        public float nodesApartDist;
        [Range(0f, 1f)] public float randomPos;
        [Range (0f, 1f)] public float randomNodes;
    }
}

