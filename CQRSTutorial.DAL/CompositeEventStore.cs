using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;
using NHibernate.Util;

namespace CQRSTutorial.DAL
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
            get { return _eventStores.First().UnitOfWork; }
            set { _eventStores.ForEach(eventStore => eventStore.UnitOfWork = value); }
        }

        public void Add(IEvent @event)
        {
            _eventStores.ForEach(eventStore => eventStore.Add(@event));
        }
    }
}