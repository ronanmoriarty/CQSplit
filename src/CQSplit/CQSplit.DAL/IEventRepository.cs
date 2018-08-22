using System;
using System.Collections.Generic;

namespace CQSplit.DAL
{
    public interface IEventRepository : IEventStore
    {
        IEvent Read(Guid id);
        IEnumerable<IEvent> GetAllEventsFor(Guid aggregateId);
    }
}