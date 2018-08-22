using CQSplit.DAL.Serialized;

namespace CQSplit.DAL
{
    public interface IEventToPublishSerializer
    {
        IEvent Deserialize(EventToPublish eventToPublish);
        EventToPublish Serialize(IEvent @event);
    }
}