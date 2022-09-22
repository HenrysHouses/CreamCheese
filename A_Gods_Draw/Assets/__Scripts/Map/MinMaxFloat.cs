//charlie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class MinMaxFloat
    {
        public float min;
        public float max;

        public float Value()
        {
            return Random.Range(min, max);
        }
    }
}

namespace Map
{
    [System.Serializable]
    public class MinMaxInt
    {
        public int min;
        public int max;

        public int Value()
        {
            return Random.Range(min, max + 1);
        }
    }
}

