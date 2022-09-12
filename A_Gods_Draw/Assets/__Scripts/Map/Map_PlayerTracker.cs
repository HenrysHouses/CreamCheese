//CHARLIE SCRIPT

using UnityEngine;
using System.Linq;
using System;

namespace Map
{
    public class Map_PlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelect = false;
        public float enterNodeDelay = 1f;
        public Map_Manager mapManager;
        public Map_View view;

        public static Map_PlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(Map_Nodes mapNode)
        {
            if (Locked)
            {
                return;
            }

            Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                //the player has not selected the node yet, they can select any of the nodes with y = 0
                if (mapNode.Node.point.Y == 0)
                {
                    SendPlayerToNode(mapNode);
                }
                else
                {
                    PlayerWarningNodeNotAccasable();
                }
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                {
                    SendPlayerToNode(mapNode);
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
            mapManager.CurrentMap.path.Add(map_Nodes.Node.point);
            mapManager.SavingMap();

            view.SetPickableNodes();
            view.SetPathColor();
            map_Nodes.ShowSwirlAnimation();
        }

        private static void EnterNode(Map_Nodes mapNode)
        {
            Debug.Log("Node: " + mapNode.Node.blueprintName + " of type " + mapNode.Node.nodeType);

            switch (mapNode.Node.nodeType)
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

