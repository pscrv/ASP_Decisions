using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_Decisions.Extensions
{
    public static class ExtensionMethods
    {

        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Func<TSource, bool> predicate)
            {
                if (condition)
                    return source.Where(predicate).AsQueryable();
                else
                    return source;
            }

    }
}