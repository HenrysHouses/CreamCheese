//CHARLIE SCRIPT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Drawing;

namespace Map
{
    public class Map_PlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelect = false;
        public float enterNodeDelay = 1f;
        public Map_Manager mapM;
        public Map_View view;

        public static Map_PlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(Map_Nodes map_Nodes)
        {
            if (Locked)
            {
                return;
            }

            if(mapM.CurrentMap.path.Count == 0)
            {
                if(map_Nodes.Node.point.Y == 0)
                {
                    SendPlayerToNode(map_Nodes);
                }
                else
                {
                    PlayerWarningNodeNotAccasable();
                }
            }
            else
            {
                var currentPoint = mapM.CurrentMap.path[mapM.CurrentMap.path.Count - 1];
                var currentNode = mapM.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(map_Nodes.Node.point)))
                {
                    SendPlayerToNode(map_Nodes);
                }
                else
                {
                    PlayerWarningNodeNotAccasable();
                }
            }
        }

        private void SendPlayerToNode(Map_Nodes map_Nodes)
        {
            Locked = lockAfterSelect;
            mapM.CurrentMap.path.Add(map_Nodes.Node.point);
            mapM.SavingMap();

            view.SetPickableNodes();
            view.SetPathColor();
            map_Nodes.ShowSwirlAnimation();
        }

        private static void EnterNode(Map_Nodes map_Nodes)
        {
            Debug.Log("Node: " + map_Nodes.Node.blueprintName + " of type " + map_Nodes.Node.nodeType);

            switch (map_Nodes.Node.nodeType)
            {
                case NodeType.Enemy:
                    break;
                case NodeType.Elite:
                    break;
                case NodeType.RestPlace:
                    break;
                case NodeType.Reward:
                    break;
                case NodeType.Boss:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayerWarningNodeNotAccasable()
        {
            Debug.Log("Node can not be accessed");
        }
    }
}

