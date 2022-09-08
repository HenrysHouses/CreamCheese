//Charlie Script 

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Map
{
    public static class Map_Generator
    {
        private static Map_Configuration config;

        private static readonly List<NodeType> RandomNodes = new List<NodeType> { NodeType.RestPlace, NodeType.Reward, NodeType.Enemy };
        private static List<float> layerDist;
        private static List<List<Point>> paths;

        private static readonly List<List<Node>> nodes = new List<List<Node>>();

        public static Map GetMap(Map_Configuration configuration)
        {
            if (configuration == null)
            {
                Debug.Log("nulled");
                return null;
            }

            config = configuration;
            nodes.Clear();

            GenerateLayerDist();

            for(var i = 0; i < configuration.layers.Count; i++)
            {
                PlaceLayer(i);
            }

            GeneratePath();
            RandomNodePos();
            SetupConnections();
            RemoveCrossConnections();

            var nodeList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            var bossNodeName = configuration.nodeBlueprints.Where(b => b.nodeType == NodeType.Boss).ToList().Random().name;
            return new Map(configuration.name, bossNodeName, nodeList, new List<Point>());
        }

        private static void GenerateLayerDist()
        {
            layerDist = new List<float>();

            foreach(var layer in config.layers)
            {
                layerDist.Add(layer.distLayers.Value());
            }
        }

        private static float GetDistToPlayer(int layerIndex)
        {
            if(layerIndex < 0 || layerIndex > layerDist.Count)
            {
                return 0f;
            }

            return layerDist.Take(layerIndex + 1).Sum();
        }

        private static void PlaceLayer(int layerIndex)
        {
            var layer = config.layers[layerIndex];
            var nodesOnThisLayer = new List<Node>();

            var offset = layer.nodesApartDist * config.GridWidth / 2f;

            for(var i = 0; i < config.GridWidth; i++)
            {
                var nodeType = Random.Range(0f, 1f) < layer.randomNodes ? GetRandomNodes() : layer.nodeType;
                var blueprintName = config.nodeBlueprints.Where(b => b.nodeType == nodeType).ToList().Random().name;

                var node = new Node(nodeType, blueprintName, new Point(i, layerIndex))
                {
                    pos = new Vector2(-offset + i * layer.nodesApartDist, GetDistToPlayer(layerIndex))
                };

                nodesOnThisLayer.Add(node);
            }

            nodes.Add(nodesOnThisLayer);
        }

        private static void RandomNodePos()
        {
            for(var index = 0; index < nodes.Count; index++)
            {
                var list = nodes[index];
                var layer = config.layers[index];
                var distToNextLayer = index + 1 >= layerDist.Count
                    ? 0f
                    : layerDist[index + 1];
                var distToPrevLayer = layerDist[index];

                foreach(var node in list)
                {
                    var xRandom = Random.Range(-1f, 1f);
                    var yRandom = Random.Range(-1f, 1f);

                    var x = xRandom * layer.nodesApartDist / 2f;
                    var y = yRandom < 0 ? distToPrevLayer * yRandom / 2f : distToNextLayer * yRandom / 2f;

                    node.pos += new Vector2(x, y) * layer.randomPos;
                }
            }
        }

        private static void SetupConnections()
        {
            foreach(var path in paths)
            {
                for(var i = 0; i < path.Count; i++)
                {
                    var node = GetNode(path[i]);
                    
                    if(i > 0)
                    {
                        var nextNode = GetNode(path[i - 1]);
                        nextNode.AddingIncoming(node.point);
                        node.AddingOutgoing(nextNode.point);
                    }

                    if(i < path.Count - 1)
                    {
                        var previousNode = GetNode(path[i + 1]);
                        previousNode.AddingOutgoing(node.point);
                        node.AddingIncoming(previousNode.point);
                    }
                }
            }
        }

        private static void RemoveCrossConnections()
        {

        }

        private static Node GetNode(Point p)
        {
            if(p.Y >= nodes.Count)
            {
                return null;
            }

            if(p.X >= nodes[p.Y].Count)
            {
                return null;
            }

            return nodes[p.Y][p.X];
        }

        private static Point GetFinalNode()
        {
            var y = config.layers.Count - 1;
            if(config.GridWidth % 2 == 1)
            {
                return new Point(config.GridWidth / 2, y);
            }

            return Random.Range(0, 2) == 0
                ? new Point(config.GridWidth / 2, y)
                : new Point(config.GridWidth / 2 - 1, y);
        }

        private static void GeneratePath()
        {
            var finalNode = GetFinalNode();
            paths = new List<List<Point>>();
        }

        private static NodeType GetRandomNodes()
        {
            return RandomNodes[Random.Range(0, RandomNodes.Count)];
        }
    }
}