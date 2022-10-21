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
        MeshRenderer mr;
        public SpriteRenderer visitedSprite;
        public Image visitedImage; //image showing that you have visited that node

        public Node Node { get; private set; }
        // ! TEMPORARY
        public Node tempNode {set { Node = value;}}
        public NodeBlueprint Blueprint { get; private set; }

        private float initialScale;
        private const float HoverScaleFactor = 1.2f;
        private float mouseDownTime;
        private const float maxClickDuration = 0.5f;
        
        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;

            GameObject mdl;
            if(blueprint.models)
            {
                mdl = Instantiate(blueprint.models);
                mdl.transform.SetParent(transform, false);
                mdl.transform.localPosition = new Vector3();
                mr = mdl.GetComponent<MeshRenderer>();
            }

            if(sr)
                sr.sprite = blueprint.sprite;

            if (node.nodeType == NodeType.Boss)
            {
                transform.localScale *= 1.5f;
            }

            if(sr)
            {
                initialScale = sr.transform.localScale.x;
                visitedSprite.color = Map_View.instance.visitedColor;
                visitedSprite.gameObject.SetActive(false);
            }

            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates states)
        {
            if(sr)
                visitedSprite.gameObject.SetActive(false);

            switch (states)
            {
                case NodeStates.Locked:
                    if(sr)
                        sr.color = Map_View.instance.lockedColor;
                    if(mr)
                        mr.material.color = Map_View.instance.lockedColor;
                    break;

                case NodeStates.Visited:
                    if(sr)
                    {
                        sr.color = Map_View.instance.visitedColor;
                        visitedSprite.gameObject.SetActive(true);
                    }
                    if(mr)
                        mr.material.color = Map_View.instance.visitedColor;
                    break;

                case NodeStates.Taken:
                    if(sr)
                        sr.color = Map_View.instance.AvailableColor;
                    if(mr)
                        mr.material.color = Map_View.instance.AvailableColor;
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

