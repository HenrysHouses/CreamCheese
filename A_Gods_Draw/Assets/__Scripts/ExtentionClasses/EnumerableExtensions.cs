using System.Linq;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        var r = new System.Random();
        var list = enumerable as IList<T> ?? enumerable.ToList();
        return list.ElementAt(r.Next(0, list.Count()));
    }
}