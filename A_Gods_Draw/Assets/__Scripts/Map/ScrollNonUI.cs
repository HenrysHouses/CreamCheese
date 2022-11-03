//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Map
{
    public class ScrollNonUI : MonoBehaviour
    {
        public float twBackDuration = 0.3f;
        public EasingMode backEase;

        public bool freezeX;
        public MinMaxFloat xConst = new MinMaxFloat();
        public bool freezeY;
        public MinMaxFloat yConst = new MinMaxFloat();
        
        private Vector2 offset;
        private Vector3 pointerDisplacement;
        private float zDisplacement;
        private bool isDragging;
        private Camera main;

        private void Awake()
        {
            main = Camera.main;
            zDisplacement = -main.transform.position.z + transform.position.z;
        }

        public void OnMouseDown()
        {
            pointerDisplacement = -transform.position + MouseInWorldCoordinates();
            //transform
            isDragging = true;
        }

        public void OnMouseUp()
        {
            isDragging = false;
            Back();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isDragging)
            {
                return;
            }

            var mousePos = MouseInWorldCoordinates();
            transform.position = new Vector3(freezeX ? transform.position.x : mousePos.x - pointerDisplacement.x,
                                             freezeY ? transform.position.y : mousePos.y - pointerDisplacement.y,
                                             transform.position.z);
        }

        private Vector3 MouseInWorldCoordinates()
        {
            var screenMousePos = Input.mousePosition;
            screenMousePos.z = zDisplacement;
            return main.ScreenToWorldPoint(screenMousePos);
        }

        private void Back()
        {
            if (freezeY)
            {
                if(transform.localPosition.x >= xConst.min && transform.localPosition.x <= xConst.max)
                {
                    return;
                }

                var targetX = transform.localPosition.x < xConst.min ? xConst.min : xConst.max;
                //transform
            }
            else if (freezeX)
            {
                if(transform.localPosition.y >= yConst.min && transform.localPosition.y <= yConst.max)
                {
                    return;
                }

                var targetY = transform.localPosition.y < yConst.min ? yConst.min : yConst.max;
                //transform
            }
        }
    }
}

