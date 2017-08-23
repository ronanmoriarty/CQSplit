using System;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;

namespace Cafe.Domain.Commands
{
    public class CloseTab : ICloseTab
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
