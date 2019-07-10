﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AX.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Iterates through collection items invoking action(item)
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
        /// Iterates through collection items removing all that matches predicate
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
    }
}