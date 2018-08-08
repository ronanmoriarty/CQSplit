using System;
using CQ.Core;

namespace Cafe.Waiter.Events
{
    public class DrinksNotOutstanding : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}
