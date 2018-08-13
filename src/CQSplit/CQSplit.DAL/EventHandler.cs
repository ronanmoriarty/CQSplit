using System.Collections.Generic;
using CQSplit.Core;

namespace CQSplit.DAL
{
    public class EventHandler : IEventHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEventStore _eventStore;

        public EventHandler(IUnitOfWorkFactory unitOfWorkFactory, IEventStore eventStore)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _eventStore = eventStore;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create().Enrolling(_eventStore))
            {
                unitOfWork.ExecuteInTransaction(() =>
                {
                    foreach (var @event in events)
                    {
                        _eventStore.Add(@event);
                    }
                });
            }
        }
    }
}
