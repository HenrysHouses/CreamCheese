//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class DottetPath : MonoBehaviour
    {
        public bool scaleInUpdate = false;
        private LineRenderer lineRenderer;
        private Renderer rend;

        // Start is called before the first frame update
        private void Start()
        {
            ScaleMat();
            enabled = scaleInUpdate;
        }

        public void ScaleMat()
        {
            lineRenderer = GetComponent<LineRenderer>();
            rend = GetComponent<Renderer>();

            rend.material.mainTextureScale = new Vector2(Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount -1)) / lineRenderer.widthMultiplier, 1);
        }

        // Update is called once per frame
        private void Update()
        {
            rend.material.mainTextureScale = new Vector2(Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1)) / lineRenderer.widthMultiplier, 1);
        }
    }
}

