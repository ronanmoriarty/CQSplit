using System;
using Cafe.Waiter.Contracts.Commands;

namespace Cafe.Waiter.Domain.Commands
{
    public class CloseTabCommand : ICloseTabCommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
