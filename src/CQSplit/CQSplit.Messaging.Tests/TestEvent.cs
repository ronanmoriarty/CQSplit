using System;
using System.Diagnostics.CodeAnalysis;
using CQSplit.Core;

namespace CQSplit.Messaging.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}