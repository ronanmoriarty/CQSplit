using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishSerializer
    {
        IEvent Deserialize(EventToPublish eventToPublish);
        EventToPublish Serialize(IEvent @event);
    }
}