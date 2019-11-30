using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T obj in collection)
            {
                action.Invoke(obj);
            }
        }

        /// <summary>
        /// Enables linq like enumeration over Dictionary, etc.
        /// </summary>
        public static void ForEach<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> collection, Action<TValue> action)
        {
            foreach (KeyValuePair<TKey, TValue> obj in collection)
            {
                action.Invoke(obj.Value);
            }
        }

        /// <summary>
        /// Enables linq like where over Dictionary, etc.
        /// </summary>
        public static IEnumerable<KeyValuePair<TKey, TValue>> Where<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> collection, Predicate<TValue> action)
        {
            return collection.Where(itm => action(itm.Value));
        }
    }
}