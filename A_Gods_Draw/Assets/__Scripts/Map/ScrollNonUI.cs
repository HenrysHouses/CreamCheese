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
        public bool freezeZ;
        public MinMaxFloat zConst = new MinMaxFloat();
        public Vector2 ScrollMinMaxBounds;

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
            
            Vector3 desiredPos = new Vector3(freezeX ? transform.localPosition.x : mousePos.x - pointerDisplacement.x,
                                             freezeY ? transform.localPosition.y : mousePos.y - pointerDisplacement.y,
                                             freezeZ ? transform.localPosition.z : mousePos.z - pointerDisplacement.z);
            desiredPos = new Vector3(Mathf.Clamp(desiredPos.x, ScrollMinMaxBounds.x, ScrollMinMaxBounds.y),
                                     Mathf.Clamp(desiredPos.y, ScrollMinMaxBounds.x, ScrollMinMaxBounds.y),
                                     Mathf.Clamp(desiredPos.z, ScrollMinMaxBounds.x, ScrollMinMaxBounds.y));
            transform.localPosition = desiredPos;            
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
            else if (freezeZ)
            {
                if(transform.localPosition.z >= zConst.min && transform.localPosition.z <= zConst.max)
                {
                    return;
                }

                var targetZ = transform.localPosition.z < zConst.min ? zConst.min : zConst.max;
                //transform
            }
        }
    }
}

