using System;
using Cafe.Waiter.Contracts.Events;

namespace Cafe.Domain.Events
{
    public class TabClosed : ITabClosed
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }

        public decimal AmountPaid;
        public decimal OrderValue;
        public decimal TipValue;
    }
}
