//Charlie Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Drawing;
using Newtonsoft.Json;

namespace Map
{
    public class Map
    {
        public List<Node> nodes;
        public List<MapPoint> path;

        public string bossNodeName;
        public string configName;

        public Map(string configName, string bossNodeName, List<Node> nodes, List<MapPoint> path)
        {
            this.configName = configName;
            this.bossNodeName = bossNodeName;
            this.nodes = nodes;
            this.path = path;
        }

        public Node GetBossNode()
        {
            return nodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
        }

        public float DistLayers() //distance between the first and last layers
        {
            var bossNode = GetBossNode();
            var firstLayerNode = nodes.FirstOrDefault(n => n.point.y == 0);

            if(bossNode == null || firstLayerNode == null)
            {
                return 0f;
            }

            return bossNode.pos.y - firstLayerNode.pos.y;
        }

        public Node GetNode(MapPoint point)
        {
            return nodes.FirstOrDefault(n => n.point.Equals(point));
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
