using System.Collections.Generic;

namespace CQSplit.DAL
{
    public class EventHandler : IEventHandler
    {
        private readonly IEventStore _eventStore;

        public EventHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            _eventStore.UnitOfWork.ExecuteInTransaction(() =>
            {
                foreach (var @event in events)
                {
                    _eventStore.Add(@event);
                }
            });
        }
    }
}
