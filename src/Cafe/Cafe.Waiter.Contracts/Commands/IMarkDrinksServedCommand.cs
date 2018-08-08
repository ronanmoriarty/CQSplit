using System.Collections.Generic;
using CQ.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IMarkDrinksServedCommand : ICommand
    {
        List<int> MenuNumbers { get; set; }
    }
}