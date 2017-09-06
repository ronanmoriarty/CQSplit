using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Contracts.Events;

namespace Cafe.Domain.Events
{
    public class DrinksOrdered : IDrinksOrdered
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
