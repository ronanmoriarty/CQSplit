using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Domain.Events
{
    public class DrinksOrdered : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
