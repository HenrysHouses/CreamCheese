//Charlie Script 02.09.22

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Drawing;

namespace Map
{
    public class Node
    {
        public readonly Point point;
        public readonly List<Point> incoming = new List<Point>();
        public readonly List<Point> outgoing = new List<Point>();
        public readonly NodeType nodeType;
        public readonly string blueprintName;
        public Vector2 pos;

        public Node (NodeType nodeType, string blueprintName, Point point)
        {
            this.nodeType = nodeType;
            this.blueprintName = blueprintName;
            this.point = point;
        }

        #region ADDING points
        public void AddingIncoming(Point point)
        {
            if(incoming.Any(x => x.Equals(point)))
            {
                return;
            }
            incoming.Add(point);
        }

        public void AddingOutgoing(Point point)
        {
            if (outgoing.Any(x => x.Equals(point)))
            {
                return;
            }
            outgoing.Add(point);
        }
        #endregion

        #region REMOVING points
        public void RemovingIncoming()
        {
            incoming.RemoveAll(x => x.Equals(point));
        }

        public void RemovingOutgoing()
        {
            outgoing.RemoveAll(x => x.Equals(point));
        }

        #endregion

        public bool ItHasNoConnections()
        {
            return incoming.Count == 0 && outgoing.Count == 0;
        }
    }
}

