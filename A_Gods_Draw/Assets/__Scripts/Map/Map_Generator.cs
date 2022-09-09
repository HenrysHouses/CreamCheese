//Charlie Script 

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
        { NodeType.RestPlace, NodeType.Reward, NodeType.Enemy };

        private static List<float> layerDist;
        private static List<List<Point>> paths;

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

            for(var i = 0; i < configuration.layers.Count; i++)
            {
                PlaceLayer(i);
            }

            GeneratePath();
            RandomNodePos();
            SetupConnections();
            RemoveCrossConnections();

            var nodeList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            var bossNodeName = config.nodeBlueprints.Where(b => b.nodeType == NodeType.Boss).ToList().Random().name;
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
            for(var i = 0; i < config.GridWidth - 1; i++)
            {
                for (var j = 0; j < config.layers.Count - 1; j++)
                {
                    var node = GetNode(new Point(i, j));
                    if (node == null || node.ItHasNoConnections())
                    {
                        continue;
                    }

                    var right = GetNode(new Point(i + 1, j));
                    if (right == null || right.ItHasNoConnections())
                    {
                        continue;
                    }

                    var top = GetNode(new Point(i, j + 1));
                    if(top == null || top.ItHasNoConnections())
                    {
                        continue;
                    }

                    var topRight = GetNode(new Point(i + 1, j + 1));
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

                    var random = Random.Range(0f, 1f);
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
            var numberOfStartingNodes = config.numOfStartingNodes.Value();
            var numberOfBossNodes = config.numOfPreBossNodes.Value();

            var candidateXs = new List<int>();
            for(var i = 0; i < config.GridWidth; i++)
            {
                candidateXs.Add(i);
            }

            candidateXs.Shuffling();
            var preBossX = candidateXs.Take(numberOfBossNodes);
            var preBossPoints = (from x in preBossX select new Point(x, finalNode.Y - 1)).ToList();
            var attempts = 0;

            foreach(var point in preBossPoints)
            {
                var path = Path(point, 0, config.GridWidth);
                path.Insert(0, finalNode);
                paths.Add(path);
                attempts++;
            }

            while(!PathToDiffPoints(paths, numberOfStartingNodes) && attempts < 100)
            {
                var rndPreBossPoint = preBossPoints[Random.Range(0, preBossPoints.Count)];
                var path = Path(rndPreBossPoint, 0, config.GridWidth);
                path.Insert(0, finalNode);
                paths.Add(path);
                attempts++;
            }
        }

        private static bool PathToDiffPoints(IEnumerable<List<Point>> paths, int n)
        {
            return (from path in paths select path[path.Count - 1].X).Distinct().Count() >= n;
        }

        private static List<Point> Path(Point from, int toY, int width, bool firstStepUnconstrained = false)
        {
            if(from.Y == toY)
            {
                return null;
            }

            var dir = from.Y > toY ? -1 : 1;
            var path = new List<Point> { from };

            while (path[path.Count - 1].Y != toY)
            {
                var lastPoint = path[path.Count - 1];
                var candidateXs = new List<int>();

                if(firstStepUnconstrained && lastPoint.Equals(from))
                {
                    for(var i = 0; i < width; i++)
                    {
                        candidateXs.Add(i);
                    }
                }
                else
                {
                    candidateXs.Add(lastPoint.X);

                    if(lastPoint.X - 1 >= 0)
                    {
                        candidateXs.Add(lastPoint.X - 1);
                    }

                    if(lastPoint.X + 1 < width)
                    {
                        candidateXs.Add(lastPoint.X + 1);
                    }
                }

                var nextPoint = new Point(candidateXs[Random.Range(0, candidateXs.Count)], lastPoint.Y + dir);
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