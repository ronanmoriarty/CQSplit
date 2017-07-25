using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder : ICommandWithAggregateId
    {
        public List<OrderedItem> Items { get; set; }
        public int AggregateId { get; set; }
    }
}
