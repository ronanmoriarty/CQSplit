using System;
using CQ.Core;

namespace Cafe.DAL.Tests
{
    public class TestEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }
}