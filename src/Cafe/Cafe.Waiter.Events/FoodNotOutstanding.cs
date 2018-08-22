using System;
using CQSplit;

namespace Cafe.Waiter.Events
{
    public class FoodNotOutstanding : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}