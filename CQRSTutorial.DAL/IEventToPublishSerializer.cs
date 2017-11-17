using CQRSTutorial.Core;
using CQRSTutorial.DAL.Serialized;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishSerializer
    {
        IEvent Deserialize(EventToPublish eventToPublish);
        EventToPublish Serialize(IEvent @event);
    }
}