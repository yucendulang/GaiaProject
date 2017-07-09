using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Util
{
    public static class TupleListExtensions
    {
        public static void Add<T1, T2>(
        this IList<Tuple<T1, T2>> list, T1 item1, T2 item2)
        {
            list.Add(Tuple.Create(item1, item2));
        }
    }


    public static class ListExtensions
    {
        public static T RandomRemove<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }
            var i=(new Random()).Next(list.Count);
            var result = list[i];
            list.RemoveAt(i);
            return result;
        }
    }
}
