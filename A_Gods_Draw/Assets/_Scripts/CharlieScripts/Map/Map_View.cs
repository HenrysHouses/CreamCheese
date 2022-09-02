using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Map
{
    public class Map_View : MonoBehaviour
    {
        public enum MapOrientations
        {

        }

        public MapOrientations orientations;
        public Map_Manager mapManager;

        public GameObject linePrefab;

        public static Map_View instance;

        private void Awake()
        {
            instance = this;
        }

        private void ClearMap()
        {
            Map_Nodes.Clear();
            LineConnection.Clear();
        }

        public void MapShow(Map m)
        {

        }

        #region CREATION of map
        private void CreateBackground(Map m)
        {

        }

        private void CreateParent()
        {

        }

        private void CreateNode(IEnumerable<Node> nodes)
        {

        }

        private Map_Nodes CreateMapNode(Node node)
        {

        }

        public void SetPickableNodes()
        {

        }

        public void SetPathColor()
        {

        }

        private void Orientation()
        {

        }

        private void DrawPath()
        {

        }

        private void ResetRotation() //node rot
        {

        }
        #endregion

        private Map_Nodes GetNodes(Dot dot)
        {

        }

        private Map_Configuration GetConfiguration(string configName)
        {

        }

        public NodeBlueprint GetNodeBlueprint(NodeType nodeType)
        {

        }

        public NodeBlueprint GetNodeBlueprint(string blueprintName)
        {

        }
    }
}
