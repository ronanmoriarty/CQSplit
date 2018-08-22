using System.Collections.Generic;
using CQSplit;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IMarkDrinksServedCommand : ICommand
    {
        List<int> MenuNumbers { get; set; }
    }
}