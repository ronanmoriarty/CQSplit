using System;

namespace CQSplit.Serialization
{
    public class Event
    {
        public virtual Guid Id { get; set; }
        public virtual Guid AggregateId { get; set; }
        public virtual Guid CommandId { get; set; }
        public virtual string EventType { get; set; }
        public virtual string Data { get; set; }
        public virtual DateTime Created { get; set; }
    }
}