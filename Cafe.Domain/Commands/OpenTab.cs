using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class OpenTab : ICommand
    {
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
        public Guid AggregateId { get; set; }
    }
}