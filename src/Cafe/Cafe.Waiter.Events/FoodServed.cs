using System;
using System.Collections.Generic;
using CQSplit;

namespace Cafe.Waiter.Events
{
    public class FoodServed : IEvent
     {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }

        public List<int> MenuNumbers;
    }
}