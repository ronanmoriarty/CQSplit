﻿using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;

namespace Cafe.Waiter.Commands
{
    public class MarkDrinksServedCommand : IMarkDrinksServedCommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<int> MenuNumbers { get; set; }
    }
}
