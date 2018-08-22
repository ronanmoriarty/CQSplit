namespace CQSplit.Serialization
{
    public interface IEventToPublishSerializer
    {
        IEvent Deserialize(EventToPublish eventToPublish);
        EventToPublish Serialize(IEvent @event);
    }
}