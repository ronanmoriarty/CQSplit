using System;
using Cafe.Waiter.Contracts.Events;

namespace Cafe.Domain.Events
{
    public class DrinksNotOutstanding : IDrinksNotOutstanding
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}
