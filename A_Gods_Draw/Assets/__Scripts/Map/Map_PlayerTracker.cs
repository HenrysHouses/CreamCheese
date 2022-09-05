//CHARLIE SCRIPT
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

            if(mapM.currentMap.path.Count == 0)
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

            }
        }

        private void SendPlayerToNode(Map_Nodes map_Nodes)
        {
            Locked = lockAfterSelect;
            mapM.currentMap.path.Add(map_Nodes.Node.point);
            mapM.SavingMap();

            view.SetPickableNodes();
            //view.SetPathColors();
            //map_Nodes.ShowSwirlAnim();
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
}*/

