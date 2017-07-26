using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class OpenTab : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}