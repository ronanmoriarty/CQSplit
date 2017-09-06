using System;
using Cafe.Waiter.Contracts.Events;

namespace Cafe.Domain.Events
{
    public class FoodNotOutstanding : IFoodNotOutstanding
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}