using System;
using CQSplit.Core;

namespace Cafe.Waiter.Command.Service.Tests.Sql
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