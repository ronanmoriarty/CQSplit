using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class FoodOrdered : IEvent
    {
        public Guid Id { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
