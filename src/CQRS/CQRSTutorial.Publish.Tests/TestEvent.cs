using System;
using System.Diagnostics.CodeAnalysis;
using CQRSTutorial.Core;

namespace CQRSTutorial.Publish.Tests
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