//Charlie Script 02.09.22

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
        public List<Point> path;

        public string bossNodeName;
        public string configName;

        public Map(string configName, string bossNodeName, List<Node> nodes, List<Point> path)
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
            var firstLayerNode = nodes.FirstOrDefault(n => n.point.Y == 0);

            if(bossNode == null || firstLayerNode == null)
            {
                return 0f;
            }

            return bossNode.pos.y - firstLayerNode.pos.y;
        }

        public Node GetNode(Point point)
        {
            return nodes.FirstOrDefault(n => n.point.Equals(point));
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
