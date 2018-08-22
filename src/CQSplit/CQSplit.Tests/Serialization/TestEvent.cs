using System;

namespace CQSplit.Serialization.Tests
{
    public class TestEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public string StringProperty { get; set; }
    }
}