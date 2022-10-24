//CHARLIE SCRIPT

using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

namespace Map
{
    public class Map_PlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelect = false;
        public float enterNodeDelay = 1f;
        public Map_Manager mapManager;
        public Map_View view;
        public SceneTransition _SceneTransitioner;
        static SceneTransition sceneTransition;

        public static Map_PlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
            if(sceneTransition == null)
                sceneTransition = _SceneTransitioner;
        }

        public void SelectNode(Map_Nodes mapNode)
        {
            if (Locked)
            {
                return;
            }

            if (mapManager.CurrentMap.path.Count == 0)
            {
                //the player has not selected the node yet, they can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                {
                    SendPlayerToNode(mapNode); //
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
            mapManager.SavingMap(); //

            view.SetPickableNodes();
            view.SetPathColor();
            map_Nodes.ShowSwirlAnimation();
            EnterNode(map_Nodes);
        }

        /*i guess this is where we would put the player into "scene fitting to the node type"
         * so when selecting a node you enter the one meant for it*/
        private static void EnterNode(Map_Nodes mapNode)
        {
            // Debug.Log("Node: " + mapNode.Node.blueprintName + " of type " + mapNode.Node.nodeType);
            GameManager.instance.nextRewardType = mapNode.Node.nodeType;

            switch (mapNode.Node.nodeType)
            {
                case NodeType.Enemy:
                    sceneTransition.TransitionScene(false, "Combat");
                    GameManager.instance.nextCombatType = EncounterDifficulty.Easy;
                    break;
                case NodeType.Elite:
                    sceneTransition.TransitionScene(false, "Combat");
                    GameManager.instance.nextCombatType = EncounterDifficulty.elites;
                    break;
                case NodeType.RestPlace:
                    sceneTransition.TransitionScene(false, "MainMenu");
                    break;
                case NodeType.RandomReward:
                    sceneTransition.TransitionScene(false, "CardReward");
                    Debug.Log(CardType.None);
                    break;
                case NodeType.AttackReward:
                    sceneTransition.TransitionScene(false, "CardReward");
                    Debug.Log(CardType.Attack);
                    break;
                case NodeType.DefenceReward:
                    sceneTransition.TransitionScene(false, "CardReward");
                    Debug.Log(CardType.Defence);
                    break;
                case NodeType.BuffReward:
                    sceneTransition.TransitionScene(false, "CardReward");
                    Debug.Log(CardType.Buff);
                    break;
                case NodeType.GodReward:
                    sceneTransition.TransitionScene(false, "CardReward");
                    Debug.Log(CardType.God);
                    break;
                case NodeType.Boss:
                    sceneTransition.TransitionScene(false, "Combat");
                    GameManager.instance.nextCombatType = EncounterDifficulty.Boss;
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

