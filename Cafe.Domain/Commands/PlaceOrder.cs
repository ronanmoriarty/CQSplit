using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder : ICommand
    {
        public List<OrderedItem> Items { get; set; }
        public Guid AggregateId { get; set; }
    }
}
