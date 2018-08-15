using System;
using CQSplit.Core;

namespace Cafe.DAL.IntegrationTests
{
    public class TestEvent2 : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}