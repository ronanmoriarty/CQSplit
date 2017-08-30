using System;
using Cafe.Waiter.Contracts.Commands;

namespace Cafe.Domain.Commands
{
    public class OpenTabCommand : IOpenTabCommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}