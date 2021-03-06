﻿using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;

namespace Cafe.Waiter.Commands
{
    public class PlaceOrderCommand : IPlaceOrderCommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<OrderedItem> Items { get; set; }
    }
}
