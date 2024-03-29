﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AX
{
    public static class Extensions
    {
        /// <summary>
        /// Iterates through enumerable items invoking action(item)
        /// Be wary that enumarable should not change in process
        /// Returns SAME IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
            return source;

        }

        /// <summary>
        /// Removes all items that matches predicate
        /// Returns SAME ICollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static ICollection<T> RemoveAll<T>(this ICollection<T> collection, Predicate<T> predicate)
        {
            var listToRemove = collection.Where(x => predicate(x)).ToList();
            foreach (var item in listToRemove)
            {
                collection.Remove(item);
            }
            return collection;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, T item)
        {
            int index = 0;
            foreach(var entry in collection)
            {
                if (EqualityComparer<T>.Default.Equals(entry, item)) return index;
                index++;
            }
            return -1;
        }

        public static bool IsAlmostEqualsTo(this double a, double b, double epsilon = double.Epsilon)
        {
            return Math.Abs(b - a) < epsilon;
        }

        public static bool IsAlmostEqualsTo(this float a, float b, float epsilon = float.Epsilon)
        {
            return Math.Abs(b - a) < epsilon;
        }
    }
}
