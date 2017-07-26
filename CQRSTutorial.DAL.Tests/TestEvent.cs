using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL.Tests
{
    internal class TestEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }
}