using System;
using System.Collections.Generic;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder
    {
        public Guid Id { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
