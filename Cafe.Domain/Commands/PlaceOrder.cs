using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder : IPlaceOrder
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
