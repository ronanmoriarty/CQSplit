using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class MarkDrinksServed : ICommand
    {
        public List<int> MenuNumbers;
        public Guid AggregateId { get; set; }
    }
}
