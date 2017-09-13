using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventRepository : IEventStore
    {
        IEvent Read(Guid id);
        IEnumerable<IEvent> GetAllEventsFor(Guid aggregateId);
    }
}