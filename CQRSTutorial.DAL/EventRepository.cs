using System;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventRepository : IEventRepository
    {
        public void Add(IEvent @event, ISession session)
        {
            var eventDescriptor = new EventDescriptor
            {
                EventType = @event.GetType(),
                Data = JsonConvert.SerializeObject(@event)
            };
            session.SaveOrUpdate(eventDescriptor);
            UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(@event, eventDescriptor);
        }

        public IEvent Read(Guid id, ISession session)
        {
                var eventDescriptor = session.Get<EventDescriptor>(id);
                var @event = (IEvent)JsonConvert.DeserializeObject(eventDescriptor.Data, eventDescriptor.EventType);
                AssignEventIdFromEventDescriptorId(@event, eventDescriptor);
                return @event;
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(IEvent @event,
            EventDescriptor eventDescriptor)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the EventDescriptor's Id gets updated during saving.
            @event.Id = eventDescriptor.Id;
        }

        private void AssignEventIdFromEventDescriptorId(IEvent @event, EventDescriptor eventDescriptor)
        {
            @event.Id = eventDescriptor.Id;
        }
    }
}
