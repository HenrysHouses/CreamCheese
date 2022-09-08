//Charlie Script 02.09.22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class Path
    {
        public LineRenderer lineRenderer;
        public Map_Nodes from;
        public Map_Nodes to;

        public Path(LineRenderer lineRenderer, Map_Nodes from, Map_Nodes to)
        {
            this.lineRenderer = lineRenderer;
            this.from = from;
            this.to = to;
        }

        public void SetColor(Color col)
        {
            var gradient = lineRenderer.colorGradient;
            var colorKeys = gradient.colorKeys;

            for(var i = 0; i < colorKeys.Length; i++)
            {
                colorKeys[i].color = col;
            }

            gradient.colorKeys = colorKeys;
            lineRenderer.colorGradient = gradient;
        }
    }
}

