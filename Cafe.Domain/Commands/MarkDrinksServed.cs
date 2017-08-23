using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts;

namespace Cafe.Domain.Commands
{
    public class MarkDrinksServed : IMarkDrinksServed
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<int> MenuNumbers { get; set; }
    }
}
