using System.Collections.Generic;
using System.Linq;
using CQ.Core;

namespace CQ.DAL
{
    public class CompositeEventStore : IEventStore
    {
        private readonly IEnumerable<IEventStore> _eventStores;

        public CompositeEventStore(IEnumerable<IEventStore> eventStores)
        {
            _eventStores = eventStores;
        }

        public IUnitOfWork UnitOfWork
        {
            get => _eventStores.First().UnitOfWork;
            set
            {
                foreach (var eventStore in _eventStores)
                {
                    eventStore.UnitOfWork = value;
                }
            }
        }

        public void Add(IEvent @event)
        {
            foreach (var eventStore in _eventStores)
            {
                eventStore.Add(@event);
            }
        }
    }
}