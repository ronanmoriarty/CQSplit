using System;
using CQSplit;

namespace Cafe.Waiter.Command.Service.Tests.Sql
{
    public class TestEvent2 : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}