using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class CloseTab : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public decimal AmountPaid;
    }
}
