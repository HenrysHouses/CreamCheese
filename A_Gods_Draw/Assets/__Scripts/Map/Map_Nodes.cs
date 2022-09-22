//Charlie Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

namespace Map
{
    public enum NodeStates
    {
        Locked,
        Visited,
        Taken
    }
}

namespace Map
{
    public class Map_Nodes : MonoBehaviour
    {
        //for ui and sprites for nodes of the map
        public SpriteRenderer sr;
        public SpriteRenderer visitedSprite;
        public Image visitedImage; //image showing that you have visited that node

        //for models as nodes of the map
        public Renderer rd;
        public Renderer visitedNode;
        public GameObject visitedGO;

        public Node Node { get; private set; }
        public NodeBlueprint Blueprint { get; private set; }

        private float initialScale;
        private const float HoverScaleFactor = 1.2f;
        private float mouseDownTime;
        private const float maxClickDuration = 0.5f;

        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;
            sr.sprite = blueprint.sprite;
            

            if (node.nodeType == NodeType.Boss)
            {
                transform.localScale *= 1.5f;
            }

            initialScale = sr.transform.localScale.x;
            visitedSprite.color = Map_View.instance.visitedColor;
            visitedSprite.gameObject.SetActive(false);

            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates states)
        {
            visitedSprite.gameObject.SetActive(false);
            switch (states)
            {
                case NodeStates.Locked:
                    sr.color = Map_View.instance.lockedColor;
                    break;

                case NodeStates.Visited:
                    sr.color = Map_View.instance.visitedColor;
                    visitedSprite.gameObject.SetActive(true);
                    break;

                case NodeStates.Taken:
                    sr.color = Map_View.instance.lockedColor;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(states), states, null);
            }
        }

        private void OnMouseDown()
        {
           mouseDownTime = Time.time;
        }

        private void OnMouseUp()
        {
            if (Time.time - mouseDownTime < maxClickDuration)
            {
                //the player has now clicked on this mode and will continue on this path (i guess)
                Map_PlayerTracker.Instance.SelectNode(this); //
            }
        }

        public void ShowSwirlAnimation()
        {
            if (visitedImage == null)
            {
                return;
            }
        }
    }
}

