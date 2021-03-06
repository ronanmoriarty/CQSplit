﻿using System;
using CQSplit;

namespace Cafe.Waiter.Events
{
    public class TabClosed : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }

        public decimal AmountPaid;
        public decimal OrderValue;
        public decimal TipValue;
    }
}
