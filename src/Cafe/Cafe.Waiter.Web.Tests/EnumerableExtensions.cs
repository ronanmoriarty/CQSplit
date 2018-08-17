using System;
using System.Collections.Generic;

namespace Cafe.Waiter.Web.Tests
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var @object in enumerable)
            {
                action(@object);
            }
        }
    }
}