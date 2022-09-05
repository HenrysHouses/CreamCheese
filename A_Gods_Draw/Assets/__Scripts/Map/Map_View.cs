//CHARLIE SCRIPT
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Drawing;

namespace Map
{
    public class Map_View : MonoBehaviour
    {
        public enum MapOrientations
        {
            BottomToTop,
            TopToBottom,
            RightToLeft,
            LeftToRight
        }

        public MapOrientations orientations;
        public Map_Manager mapManager;

        public List<Map_Configuration> allMapConfigs;
        public GameObject nodePrefabs;

        public float orientationOffset;

        [Header("Background Settings")]
        [Tooltip("If the background sprite is null, background will not be shown")]
        public Sprite background;
        public Color32 backgroundColor = UnityEngine.Color.white;
        public float xSize;
        public float yOffset;

        public GameObject linePrefab;

        private GameObject firstParent;
        private GameObject mapParent;
        private List<List<Point>> paths;
        private Camera cam;

        public readonly List<Map_Nodes> MapNodes = new List<Map_Nodes>();
        private readonly List<Path> path = new List<Path>();

        //
        public static Map_View instance;

        private void Awake()
        {
            instance = this;
            cam = Camera.main;
        }

        private void ClearMap()
        {
            if (firstParent != null) {
                Destroy(firstParent);
            }

            MapNodes.Clear();
            path.Clear();
        }

        public void MapShow(Map m)
        {
            if (m == null) return;

            ClearMap();
            CreateParent();
            CreateMapNode(m.node);
            DrawPath();
            Orientation();
            ResetRotation();
            SetPathColor();
            CreateBackground(m);
            SetPickableNodes();
        }

        #region CREATION of map
        private void CreateBackground(Map m)
        {
            if (background == null)
            {
                return;
            }

            var bgObj = new GameObject("Background");
            bgObj.transform.SetParent(mapParent.transform);

            var bossNode = MapNodes.FirstOrDefault(node => node.Node.nodeType == NodeType.Boss);
            var span = m.DistLayers(); //distance between first and last layers

            bgObj.transform.localPosition = new Vector3(bossNode.transform.localPosition.x, span / 2f, 0f);
            bgObj.transform.localRotation = Quaternion.identity;

            var spriteRenderer = bgObj.AddComponent<SpriteRenderer>();
            spriteRenderer.color = backgroundColor;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.sprite = background;
            spriteRenderer.size = new Vector2(xSize, span + yOffset * 2f);
        }

        private void CreateParent()
        {
            firstParent = new GameObject("OuterPartParent");
            mapParent = new GameObject("MapParentScrolling");
            mapParent.transform.SetParent(firstParent.transform);

            var scrollNonUI = mapParent.AddComponent<ScrollNonUI>();
            scrollNonUI.freezeX = orientations == MapOrientations.BottomToTop || orientations == MapOrientations.TopToBottom;
            scrollNonUI.freezeY = orientations == MapOrientations.LeftToRight || orientations == MapOrientations.RightToLeft;

            var boxColl = mapParent.AddComponent<BoxCollider>();
            boxColl.size = new Vector3(100, 100, 1); //can be changed
        }

        private void CreateNode(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var mapNode = CreateMapNode(node);
                Map_Nodes.Add(mapNode);
            }
        }

        private Map_Nodes CreateMapNode(Node node)
        {
            var mapNodeObj = Instantiate(nodePrefabs, mapParent.transform);
            var mapNode = mapNodeObj.GetComponent<Map_Nodes>();
            var blueprint = GetNodeBlueprint(node.blueprintName);

            mapNode.SetUp(node, blueprint);
            mapNode.transform.localPosition = node.pos;
            return mapNode;
        }


        public void SetPickableNodes()
        {
            //here we are putting all the map nodes as locked/non pickable
            foreach (var node in MapNodes)
            {
                node.SetState(NodeStates.Locked);
            }

            if (mapManager.currentMap.path.Count == 0)
            {
                foreach (var node in MapNodes.Where(n => n.Node.point.y == 0))
                {
                    node.SetState(NodeStates.Taken);
                }
            }
            else
            {
                foreach (var point in mapManager.currentMap.path)
                {
                    var mapNodes = GetNodes(point);
                    if (mapNodes != null)
                    {
                        mapNodes.SetState(NodeStates.Visited);
                    }
                }

                var currentPoint = mapManager.currentMap.path[mapManager.currentMap.path.Count - 1];
                var currentNode = mapManager.currentMap.GetNodes(currentPoint);
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
}*/
