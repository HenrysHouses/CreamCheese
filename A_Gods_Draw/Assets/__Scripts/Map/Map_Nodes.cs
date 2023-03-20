//Charlie Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FMODUnity;
using UnityEditor;

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
        public SpriteRenderer spriteRenderer;
        MeshRenderer meshRenderer;
        [SerializeField] GameObject highlight, lockedGameObject, crackedGameObject, particleGameObject; //for showing border around the map nodes
        public SpriteRenderer visitedSprite;
        public Image visitedImage; //image showing that you have visited that node
        bool isHighlightOn;
        bool shouldHighlight;

        public Node Node { get; private set; }
        // ! TEMPORARY
        public Node tempNode {set { Node = value;}}
        public NodeBlueprint Blueprint { get; private set; }

        private float initialScale;
        private const float HoverScaleFactor = 1.2f;
        private float mouseDownTime;
        private const float maxClickDuration = 0.5f;
        public EventReference click_SFX;
        public NodeStates NodeState {get; private set;}

        private void Start()
        {
            isHighlightOn = false;
        }

        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;

            GameObject mdl; //? i think mdl stands for model
            if(blueprint.models)
            {
                mdl = Instantiate(blueprint.models);
                mdl.transform.SetParent(transform, false);
                mdl.transform.localPosition = new Vector3();
                meshRenderer = mdl.GetComponent<MeshRenderer>();
            }

            if(spriteRenderer)
                spriteRenderer.sprite = blueprint.sprite;

            if (node.nodeType == NodeType.Boss)
            {
                transform.localScale *= 1.5f;

                //if(NodeType.Boss != null)
                //{
                //    Destroy(gameObject);
                //}
            }

            if(spriteRenderer)
            {
                initialScale = spriteRenderer.transform.localScale.x;
                visitedSprite.color = Map_View.instance.visitedColor;
                visitedSprite.gameObject.SetActive(false);
            }
            highlight.SetActive(false);

            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates states)
        {
            if(spriteRenderer)
                visitedSprite.gameObject.SetActive(false);
            NodeState = states;

            switch (states)
            {
                case NodeStates.Locked:
                    shouldHighlight = false;
                    if(spriteRenderer)
                        spriteRenderer.color = Map_View.instance.lockedColor;
                    if(meshRenderer)
                    {
                        //meshRenderer.material = Map_View.instance.lockedMat;
                        meshRenderer.material.color = Map_View.instance.lockedColor;
                    }
                    break;

                case NodeStates.Visited:
                    shouldHighlight = false;
                    if(spriteRenderer)
                    {
                        spriteRenderer.color = Map_View.instance.visitedColor;
                        visitedSprite.gameObject.SetActive(true);
                    }
                    if(meshRenderer)
                    {
                        //meshRenderer.material = Map_View.instance.visitedMat;
                        meshRenderer.material.color = Map_View.instance.visitedColor;
                    }
                    crackedGameObject.SetActive(true);
                    foreach (MeshRenderer _renderer in crackedGameObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        _renderer.material.color = Map_View.instance.visitedColor;
                    }
                    break;

                case NodeStates.Taken:
                    shouldHighlight = true;
                    if(spriteRenderer)
                        spriteRenderer.color = Map_View.instance.AvailableColor;
                    if(meshRenderer)
                    {
                        //meshRenderer.material = Map_View.instance.AvailableMat;
                        meshRenderer.material.color = Map_View.instance.AvailableColor;
                    }
                    particleGameObject.SetActive(true);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(states), states, null);
            }
        }

        private void OnMouseOver() //when hovering over node it has a highlight
        {
            if (!isHighlightOn && shouldHighlight)
            {
                highlight.SetActive(true);
                isHighlightOn = true;
            }
        }

        private void OnMouseExit()
        {
            if (isHighlightOn)
            {
                highlight.SetActive(false);
                isHighlightOn = false;
            }
        }

        private void OnMouseDown()
        {
           mouseDownTime = Time.time;
           SoundPlayer.PlaySound(click_SFX,gameObject);
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

