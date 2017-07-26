using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class MarkDrinksServed : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public List<int> MenuNumbers;
    }
}
