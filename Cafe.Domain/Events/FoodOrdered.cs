using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class FoodOrdered : IEvent
    {
        public int Id { get; set; }
        public int AggregateId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
