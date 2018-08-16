using System;
using System.Collections.Generic;

namespace Cafe.Waiter.Web.IntegrationTests
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