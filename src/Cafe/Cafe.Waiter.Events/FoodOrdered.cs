using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;
using CQSplit;

namespace Cafe.Waiter.Events
{
    public class FoodOrdered : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
