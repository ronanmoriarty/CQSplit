using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class MarkFoodServed : ICommandWithAggregateId
    {
        public List<int> MenuNumbers;
        public int AggregateId { get; set; }
    }
}