using CQ.Core;
using CQ.DAL.Serialized;

namespace CQ.DAL
{
    public interface IEventToPublishSerializer
    {
        IEvent Deserialize(EventToPublish eventToPublish);
        EventToPublish Serialize(IEvent @event);
    }
}