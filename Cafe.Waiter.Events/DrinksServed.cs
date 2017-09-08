using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Events
{
    public class DrinksServed : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }

        public List<int> MenuNumbers;
    }
}
