using System;
using System.Collections.Generic;
using CQSplit.Core;

namespace CQSplit.DAL
{
    public interface IEventRepository : IEventStore
    {
        IEvent Read(Guid id);
        IEnumerable<IEvent> GetAllEventsFor(Guid aggregateId);
    }
}