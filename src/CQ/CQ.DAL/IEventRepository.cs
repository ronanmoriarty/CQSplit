using System;
using System.Collections.Generic;
using CQ.Core;

namespace CQ.DAL
{
    public interface IEventRepository : IEventStore
    {
        IEvent Read(Guid id);
        IEnumerable<IEvent> GetAllEventsFor(Guid aggregateId);
    }
}