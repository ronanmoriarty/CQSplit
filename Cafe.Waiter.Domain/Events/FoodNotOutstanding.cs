using System;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Domain.Events
{
    public class FoodNotOutstanding : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}