using System;
using System.Collections.Generic;

namespace Cafe.Domain.Events
{
    public class FoodOrdered : IEvent
    {
        public Guid Id { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
