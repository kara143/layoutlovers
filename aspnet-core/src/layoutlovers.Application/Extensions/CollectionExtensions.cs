using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace layoutlovers.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// If the conditions of the predicate do not match, an empty collection is returned
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<TSource> FilterIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate).ToList();
            else
                return new List<TSource>();
        }

        public static List<TSource> FilterIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate).ToList();
            else
                return new List<TSource>();
        }

        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return !source.IsNullOrEmpty();
        }
    }
}
