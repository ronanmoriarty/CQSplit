using System;
using System.Collections.Generic;

namespace Cafe.Waiter.Contracts.QueryResponses
{
    public interface IOpenTabsQueryResponse
    {
        IEnumerable<ITab> Tabs { get; set; }
        Guid QueryId { get; set; }
    }
}
