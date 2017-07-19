using Cafe.Domain;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.DAL
{
    public class TabRepository
    {
        private readonly IEventStore _eventStore;
        private readonly EventApplier _eventApplier;

        public TabRepository(IEventStore eventStore, EventApplier eventApplier)
        {
            _eventStore = eventStore;
            _eventApplier = eventApplier;
        }

        public Tab Get(int tabId)
        {
            var events = _eventStore.GetAllEventsFor(tabId);
            var tab = new Tab();
            foreach (var @event in events)
            {
                _eventApplier.ApplyEvent(@event, new object[] { tab });
            }

            return tab;
        }
    }
}
