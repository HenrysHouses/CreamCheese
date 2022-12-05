//Charlie Script 
// Edited by Henrik

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Map
{
    public static class Map_Generator
    {
        private static Map_Configuration config;

        private static readonly List<NodeType> RandomNodes = new List<NodeType> 
        { NodeType.RestPlace, NodeType.Elite, NodeType.BuffReward, NodeType.AttackReward, NodeType.DefenceReward, NodeType.GodReward};

        private static List<float> layerDist;
        private static List<List<MapPoint>> paths;

        private static readonly List<List<Node>> nodes = new List<List<Node>>();

        public static Map GetMap(Map_Configuration configuration)
        {
            if (configuration == null)
            {
                Debug.LogWarning("nulled");
                return null;
            }

            config = configuration;
            nodes.Clear();

            GenerateLayerDist();

            for(int i = 0; i < configuration.layers.Count; i++)
            {
                PlaceLayer(i);
            }

            GeneratePath();
            RandomNodePos();
            SetupConnections();
            RemoveCrossConnections();

            List<Node> nodeList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            string bossNodeName = config.nodeBlueprints.Where(b => b.nodeType == NodeType.Boss).ToList().Random().name;
            return new Map(configuration.name, bossNodeName, nodeList, new List<MapPoint>());
        }

        private static void GenerateLayerDist()
        {
            layerDist = new List<float>();

            foreach(Map_Layer layer in config.layers)
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
            Map_Layer layer = config.layers[layerIndex];
            List<Node> nodesOnThisLayer = new List<Node>();

            float offset = layer.nodesApartDist * config.GridWidth / 2f;

            for(int i = 0; i < config.GridWidth; i++)
            {
                NodeType nodeType = Random.Range(0f, 1f) < layer.randomNodes ? GetRandomNodes() : layer.nodeType;
                string blueprintName = config.nodeBlueprints.Where(b => b.nodeType == nodeType).ToList().Random().name;

                Node node = new Node(nodeType, blueprintName, new MapPoint(i, layerIndex))
                {
                    pos = new Vector2(-offset + i * layer.nodesApartDist, GetDistToPlayer(layerIndex))
                };
                nodesOnThisLayer.Add(node);
            }

            nodes.Add(nodesOnThisLayer);
        }

        private static void RandomNodePos()
        {
            for(int index = 0; index < nodes.Count; index++)
            {
                List<Node> list = nodes[index];
                Map_Layer layer = config.layers[index];
                float distToNextLayer = index + 1 >= layerDist.Count
                    ? 0f
                    : layerDist[index + 1];
                float distToPrevLayer = layerDist[index];

                foreach(Node node in list)
                {
                    float xRandom = Random.Range(-1f, 1f);
                    float yRandom = Random.Range(-1f, 1f);

                    float x = xRandom * layer.nodesApartDist / 2f;
                    float y = yRandom < 0 ? distToPrevLayer * yRandom / 2f : distToNextLayer * yRandom / 2f;

                    node.pos += new Vector2(x, y) * layer.randomPos;
                }
            }
        }

        private static void SetupConnections()
        {
            foreach(List<MapPoint> path in paths)
            {
                for(int i = 0; i < path.Count; i++)
                {
                    Node node = GetNode(path[i]);
                    
                    if(i > 0)
                    {
                        Node nextNode = GetNode(path[i - 1]);
                        nextNode.AddingIncoming(node.point);
                        node.AddingOutgoing(nextNode.point);
                    }

                    if(i < path.Count - 1)
                    {
                        Node previousNode = GetNode(path[i + 1]);
                        previousNode.AddingOutgoing(node.point);
                        node.AddingIncoming(previousNode.point);
                    }
                }
            }
        }

        private static void RemoveCrossConnections()
        {
            for(int i = 0; i < config.GridWidth - 1; i++)
            {
                for (int j = 0; j < config.layers.Count - 1; j++)
                {
                    Node node = GetNode(new MapPoint(i, j));
                    if (node == null || node.ItHasNoConnections())
                    {
                        continue;
                    }

                    Node right = GetNode(new MapPoint(i + 1, j));
                    if (right == null || right.ItHasNoConnections())
                    {
                        continue;
                    }

                    Node top = GetNode(new MapPoint(i, j + 1));
                    if(top == null || top.ItHasNoConnections())
                    {
                        continue;
                    }

                    Node topRight = GetNode(new MapPoint(i + 1, j + 1));
                    if(topRight == null || topRight.ItHasNoConnections())
                    {
                        continue;
                    }

                    if(!node.outgoing.Any(element => element.Equals(topRight.point)))
                    {
                        continue;
                    }

                    if(!right.outgoing.Any(element => element.Equals(top.point)))
                    {
                        continue;
                    }

                    node.AddingOutgoing(top.point);
                    top.AddingIncoming(node.point);
                    right.AddingOutgoing(topRight.point);
                    topRight.AddingIncoming(right.point);

                    float random = Random.Range(0f, 1f);
                    if(random < 0.2f)
                    {
                        node.RemovingOutgoing(topRight.point);
                        topRight.RemovingIncoming(node.point);

                        right.RemovingOutgoing(top.point);
                        top.RemovingIncoming(right.point);
                    }
                    else if(random < 0.6f)
                    {
                        node.RemovingOutgoing(topRight.point);
                        topRight.RemovingIncoming(node.point);
                    }
                    else
                    {
                        right.RemovingOutgoing(top.point);
                        top.RemovingIncoming(right.point);
                    }
                }
            }
        }

        private static Node GetNode(MapPoint p)
        {
            if(p.y >= nodes.Count)
            {
                return null;
            }
            if(p.x >= nodes[p.y].Count)
            {
                return null;
            }

            return nodes[p.y][p.x];
        }

        private static MapPoint GetFinalNode()
        {
            int y = config.layers.Count - 1;
            if(config.GridWidth % 2 == 1)
            {
                return new MapPoint(config.GridWidth / 2, y);
            }

            return Random.Range(0, 2) == 0
                ? new MapPoint(config.GridWidth / 2, y)
                : new MapPoint(config.GridWidth / 2 - 1, y);
        }

        private static void GeneratePath()
        {
            MapPoint finalNode = GetFinalNode();
            paths = new List<List<MapPoint>>();
            int numberOfStartingNodes = config.numOfStartingNodes.Value();
            int numberOfBossNodes = config.numOfPreBossNodes.Value();

            List<int> candidateXs = new List<int>();
            for(int i = 0; i < config.GridWidth; i++)
            {
                candidateXs.Add(i);
            }

            candidateXs.Shuffling();
            IEnumerable<int> preBossX = candidateXs.Take(numberOfBossNodes);
            List<MapPoint> preBossPoints = (from x in preBossX select new MapPoint(x, finalNode.y - 1)).ToList();
            int attempts = 0;

            foreach(MapPoint point in preBossPoints)
            {
                List<MapPoint> path = Path(point, 0, config.GridWidth);
                path.Insert(0, finalNode);
                paths.Add(path);
                attempts++;
            }

            while(!PathToDiffPoints(paths, numberOfStartingNodes) && attempts < 100)
            {
                MapPoint rndPreBossPoint = preBossPoints[Random.Range(0, preBossPoints.Count)];
                List<MapPoint> path = Path(rndPreBossPoint, 0, config.GridWidth);
                path.Insert(0, finalNode);
                paths.Add(path);
                attempts++;
            }
        }

        private static bool PathToDiffPoints(IEnumerable<List<MapPoint>> paths, int n)
        {
            return (from path in paths select path[path.Count - 1].x).Distinct().Count() >= n;
        }

        private static List<MapPoint> Path(MapPoint from, int toY, int width, bool firstStepUnconstrained = false)
        {
            if(from.y == toY)
            {
                return null;
            }

            int dir = from.y > toY ? -1 : 1;
            List<MapPoint> path = new List<MapPoint> { from };

            while (path[path.Count - 1].y != toY)
            {
                MapPoint lastPoint = path[path.Count - 1];
                List<int> candidateXs = new List<int>();

                if(firstStepUnconstrained && lastPoint.Equals(from))
                {
                    for(int i = 0; i < width; i++)
                    {
                        candidateXs.Add(i);
                    }
                }
                else
                {
                    candidateXs.Add(lastPoint.x);

                    if(lastPoint.x - 1 >= 0)
                    {
                        candidateXs.Add(lastPoint.x - 1);
                    }

                    if(lastPoint.x + 1 < width)
                    {
                        candidateXs.Add(lastPoint.x + 1);
                    }
                }

                MapPoint nextPoint = new MapPoint(candidateXs[Random.Range(0, candidateXs.Count)], lastPoint.y + dir);
                path.Add(nextPoint);
            }

            return path;
        }

        private static NodeType GetRandomNodes()
        {
            return RandomNodes[Random.Range(0, RandomNodes.Count)];
        }
    }
}