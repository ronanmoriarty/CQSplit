using System;

namespace CQSplit.Serialization
{
    public class EventToPublish
    {
        public virtual Guid Id { get; set; }
        public virtual string EventType { get; set; }
        public virtual string Data { get; set; }
        public virtual DateTime Created { get; set; }
    }
}