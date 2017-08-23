using System;
using Cafe.Waiter.Contracts.Queries;

namespace Cafe.Waiter.Query.Service.Queries
{
    public class OpenTabsQuery : IOpenTabsQuery
    {
        public Guid Id { get; set; }
    }
}
