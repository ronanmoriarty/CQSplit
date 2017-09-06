using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Events;

namespace Cafe.Domain.Events
{
    public class FoodServed : IFoodServed
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }

        public List<int> MenuNumbers;
    }
}