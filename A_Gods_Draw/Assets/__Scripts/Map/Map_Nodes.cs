//Charlie Script 02.09.22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer visitedSpriteImage;
        public Image visitedImage; //image showing that you have visited that node

        public Node Node { get; private set; }
        public NodeBlueprint Blueprint { get; private set; }

        private float initialScale;
        private const float HoverScaleFactor = 1.2f;
        private float mouseClicked;
        private const float maxClickDuration = 0.5f;

        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;
            spriteRenderer.sprite = blueprint.sprite;
            if (node.nodeType == NodeType.Boss)
            {
                transform.localScale *= 1.5f;
            }

            initialScale = spriteRenderer.transform.localScale.x;
            visitedSpriteImage.color = Map_View.instance.visitedColor;
            visitedSpriteImage.gameObject.SetActive(false);

            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates states)
        {
            visitedSpriteImage.gameObject.SetActive(false);
            switch (states)
            {
                case NodeStates.Locked:
                    spriteRenderer.color = Color.gray;
                    break;

                case NodeStates.Visited:
                    spriteRenderer.color = Color.green;
                    break;

                case NodeStates.Taken:
                    spriteRenderer.color = Color.white;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(states), states, null);
            }
        }

        private void OnMouseEnter()
        {
            //spriteRenderer.transform.Dok();
            //spriteRenderer.transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
        }

        private void OnMouseExit()
        {
            //spriteRenderer.transform.DOKill();
            //spriteRenderer.transform.DOScale(initialScale, 0.3f);
        }

        private void OnMouseDown()
        {
           mouseClicked = Time.time;
        }

        private void OnMouseUp()
        {
            if (Time.time - mouseClicked < maxClickDuration)
            {
                //the player has now clicked on this mode and will continue on this path (i guess)
                Map_PlayerTracker.Instance.SelectNode(this);
            }
        }

        public void ShowSwirlAnimation()
        {
            if (visitedImage == null)
            {
                return;
            }

            //const float fillDuration = 0.3f;
            visitedImage.fillAmount = 0;
        }
    }
}

