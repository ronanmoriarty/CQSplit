using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder : IPlaceOrder
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
