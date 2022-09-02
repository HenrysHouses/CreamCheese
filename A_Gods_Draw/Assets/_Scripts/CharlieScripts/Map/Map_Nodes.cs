using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Map ActiveMap { get; private set; }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

