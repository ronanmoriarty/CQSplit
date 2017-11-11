using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishRepository : IEventStore
    {
        IEvent Read(Guid id);
        EventsToPublishResult GetEventsAwaitingPublishing();
        void Delete(EventToPublish eventToPublish);
    }
}