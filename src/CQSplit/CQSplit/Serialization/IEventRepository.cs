using System;
using System.Collections.Generic;

namespace CQSplit.Serialization
{
    public interface IEventRepository : IEventStore
    {
        IEvent Read(Guid id);
        IEnumerable<IEvent> GetAllEventsFor(Guid aggregateId);
    }
}