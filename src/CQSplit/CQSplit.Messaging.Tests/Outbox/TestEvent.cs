using System;
using System.Diagnostics.CodeAnalysis;

namespace CQSplit.Messaging.Tests.Outbox
{
    [ExcludeFromCodeCoverage]
    public class TestEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }
}