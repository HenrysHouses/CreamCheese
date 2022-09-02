//Charlie Script 02.09.22

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Map
{
    public static class Map_Generator
    {
        private static Map_Configuration config;
        private static readonly List<NodeType> RandomNodes = new List<NodeType> { NodeType.RestPlace, NodeType.Reward, NodeType.Enemy };
        private static List<List<Point>> paths;

        private static readonly List<List<Node>> nodes = new List<List <Node>>();

        public static Map GetMap(Map_Configuration configuration)
        {
            if (configuration == null)
            {
                return null;
            }

            config = configuration;
            nodes.Clear();

            GeneratePath();
            RandomNodePos();

            var bossNodeName = config.nodeBlueprint.Where(boss.nodeType == NodeType.Boss).ToList().Random().name;
            return new Map(configuration.name, bossNodeName, nodeList, new List<Point>());
        }

        private static void GeneratePath()
        {

        }

        private static void RandomNodePos()
        {
            for(var index = 0;)
            {

            }
        }

        private static NodeType GetRandomNodes()
        {
            return RandomNodes[Random.Range(0, RandomNodes.Count)];
        }
    }
}

