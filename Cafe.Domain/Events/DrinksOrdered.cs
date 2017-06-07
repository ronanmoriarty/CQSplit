using System;
using System.Collections.Generic;

namespace Cafe.Domain.Events
{
    public class DrinksOrdered : IEvent
    {
        public Guid Id { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
