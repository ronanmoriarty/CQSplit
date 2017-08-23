using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.QueryResponses;

namespace Cafe.Waiter.Query.Service.QueryResponses
{
    public class OpenTabsQueryResponse : IOpenTabsQueryResponse
    {
        public Guid QueryId { get; set; }
        public IEnumerable<ITab> Tabs { get; set; }
    }
}
