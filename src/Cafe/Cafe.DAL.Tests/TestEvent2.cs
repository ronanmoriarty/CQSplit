using System;
using CQ.Core;

namespace Cafe.DAL.Tests
{
    public class TestEvent2 : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}