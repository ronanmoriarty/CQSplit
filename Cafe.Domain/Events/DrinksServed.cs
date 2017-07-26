using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class DrinksServed : IEvent
    {
        public int Id { get; set; }
        public Guid AggregateId { get; set; }

        public List<int> MenuNumbers;
    }
}
