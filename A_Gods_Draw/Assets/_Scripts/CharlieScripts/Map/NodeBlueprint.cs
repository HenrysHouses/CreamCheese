//Charlie Script 02.09.22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public enum NodeType //more node types can be added
    {
        Enemy,
        Reward,
        Elite,
        RestPlace,
        Boss
    }
}

namespace Map
{
    //so we can easly make the nodes
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public NodeType nodeType;
    }
}
