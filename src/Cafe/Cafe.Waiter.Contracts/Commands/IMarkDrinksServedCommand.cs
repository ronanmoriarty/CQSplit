using System.Collections.Generic;
using CQSplit.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IMarkDrinksServedCommand : ICommand
    {
        List<int> MenuNumbers { get; set; }
    }
}