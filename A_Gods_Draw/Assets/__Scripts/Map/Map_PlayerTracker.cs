//CHARLIE SCRIPT
// Edited by Henrik

using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using HH.MultiSceneTools.Examples;

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

        public bool alwaysEasy;

        private void Awake()
        {
            Instance = this;
            if (sceneTransition == null)
                sceneTransition = _SceneTransitioner;
        }

        public void SelectNode(Map_Nodes mapNode)
        {
            if (Locked)
            {
                return;
            }

            if (Map_Manager.CurrentMap.path.Count == 0)
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
                MapPoint currentPoint = Map_Manager.CurrentMap.path[Map_Manager.CurrentMap.path.Count - 1];
                Node currentNode = Map_Manager.CurrentMap.GetNode(currentPoint);

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
            Map_Manager.CurrentMap.path.Add(map_Nodes.Node.point);

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
                    int randomNummer = UnityEngine.Random.Range(0, 2);
                    if (randomNummer == 0)
                    {
                        GameManager.instance.nextCombatType = EncounterDifficulty.Easy;
                        Debug.Log("Encounter is Easy");
                    }
                    else if (randomNummer == 1)
                    {
                        GameManager.instance.nextCombatType = EncounterDifficulty.Medium;
                        Debug.Log("Encounter is Medium");
                    }


                    break;
                case NodeType.Elite:
                    sceneTransition.TransitionScene(false, "Combat");
                    GameManager.instance.nextCombatType = EncounterDifficulty.elites;
                    break;
                case NodeType.RestPlace:
                    sceneTransition.TransitionScene(false, "RestPlace");
                    break;
                //case NodeType.RandomReward:
                //    sceneTransition.TransitionScene(false, "CardReward");
                //    Debug.Log(GameManager.instance.nextRewardType);
                //    Debug.LogWarning("Should card type be none?, fix card type");
                //    break;
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
                case NodeType.RuneReward:
                    sceneTransition.TransitionScene(false, "RuneReward");
                    Debug.Log("rune scene");
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

