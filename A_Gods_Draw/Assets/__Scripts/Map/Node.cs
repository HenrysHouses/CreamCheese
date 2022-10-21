    //Charlie Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Map
{
    public class Node
    {
        public readonly MapPoint point;
        public readonly List<MapPoint> incoming = new List<MapPoint>();
        public readonly List<MapPoint> outgoing = new List<MapPoint>();

        [JsonConverter(typeof(StringEnumConverter))]
        public readonly NodeType nodeType;
        public readonly string blueprintName;
        public Vector2 pos;

        public Node (NodeType nodeType, string blueprintName, MapPoint point)
        {
            this.nodeType = nodeType;
            this.blueprintName = blueprintName;
            this.point = point;
        }

        #region ADDING points
        public void AddingIncoming(MapPoint point)
        {
            if (incoming.Any(element => element.Equals(point)))
            {
                return;
            }
            incoming.Add(point);
        }

        public void AddingOutgoing(MapPoint point)
        {
            if (outgoing.Any(element => element.Equals(point)))
            {
                return;
            }
            outgoing.Add(point);
        }
        #endregion

        #region REMOVING points
        public void RemovingIncoming(MapPoint point)
        {
            incoming.RemoveAll(element => element.Equals(point));
        }

        public void RemovingOutgoing(MapPoint point)
        {
            outgoing.RemoveAll(element => element.Equals(point));
        }

        #endregion

        public bool ItHasNoConnections()
        {
            return incoming.Count == 0 && outgoing.Count == 0;
        }
    }
}

