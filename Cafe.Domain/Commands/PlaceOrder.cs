using System;
using System.Collections.Generic;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder
    {
        public Guid TabId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
