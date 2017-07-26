using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class PlaceOrder : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
