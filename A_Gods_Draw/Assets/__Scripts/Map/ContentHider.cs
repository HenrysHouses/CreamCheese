using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class ContentHider : MonoBehaviour
    {
        [SerializeField] Collider col;
        [SerializeField] MeshRenderer mr;
        public bool viewState;
        public bool log;

        void Start()
        {
            if(!mr)
                mr = GetComponentInChildren<MeshRenderer>();
        }

        void Update()
        {
            Vector2 bounds = Map_View.instance.ScrollBounds; // -1.17 -> 0.86

            switch(Map_View.instance.orientations)
            {
                case Map_View.MapOrientations.BackToForward:
                case Map_View.MapOrientations.ForwardToBack:
                    if(transform.position.z < bounds.y && transform.position.z > bounds.x)
                    {
                        if(!viewState)
                            setViewState(true);
                    }
                    else
                        if(viewState)
                            setViewState(false);
                    break;
                case Map_View.MapOrientations.BottomToTop:
                case Map_View.MapOrientations.TopToBottom:
                    if(transform.position.y < bounds.y && transform.position.y > bounds.x)
                    {
                        if(!viewState)
                            setViewState(true);
                    }
                    else
                        if(viewState)
                            setViewState(false);
                    break;
                case Map_View.MapOrientations.LeftToRight:
                case Map_View.MapOrientations.RightToLeft:
                    if(transform.position.x < bounds.y && transform.position.x > bounds.x)
                    {
                        if(!viewState)
                            setViewState(true);
                    }
                    else
                        if(viewState)
                            setViewState(false);
                    break;
            }
        }

        void setViewState(bool state)
        {
            viewState = state;
            mr.enabled = viewState;
            if(col)
                col.enabled = viewState;
        }
    }
}