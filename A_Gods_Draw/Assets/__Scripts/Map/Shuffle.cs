//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Map
{
    public static class Shuffle
    {
        private static System.Random range = new System.Random();
        
        public static void Shuffling<T>(this IList<T> list)
        {
            int n = list.Count;

            while(n > 1)
            {
                n--;
                int v = range.Next(n + 1);
                T value = list[v];
                list[v] = list[n];
                list[n] = value;
            }
        }

        public static T Random<T>(this IList<T> list)
        {
            return list[range.Next(list.Count)];
        }

        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(list.Count < elementsCount ? list.Count : elementsCount).ToList();
        }
    }
}

