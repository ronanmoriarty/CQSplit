using System.Collections.Generic;

namespace Cafe.Domain.Events
{
    public class FoodOrdered : ITabEvent
    {
        public int Id { get; set; }
        public int TabId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
