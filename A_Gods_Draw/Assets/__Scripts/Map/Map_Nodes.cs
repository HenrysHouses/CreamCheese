//Charlie Script 02.09.22

/*using System.Collections;
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

        private float mouseClicked;
        private const float clickDuration = 0.5f;

        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;
            //spriteRenderer.sprite = blueprint.sprite;

            if(node.nodeType == NodeType.Boss)
            {

            }

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
                    //throw new ArgumentOutOfRangeExepction(nameof(states), states, null);
                    break;
            }
        }

        private void OnMouseDown()
        {
           mouseClicked = Time.time;
        }

        private void OnMouseUp()
        {
            if (Time.time - mouseClicked < clickDuration)
            {
                //the player has now clicked on this mode and will continue on this path (i guess)

            }
        }
    }
}*/

